using BigProject.Devices.Arm.Config;
using BigProject.Devices.Arm.Kinematic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigProject.Devices.Arm.Kinematic
{
    public class Dof6kinematic
    {
        public const double RAD_TO_DEG = 57.295777754771045f;
        double[,] DH_matrix;
        double[] L1_base = new double[3];
        double[] L2_arm = new double[3];
        double[] L3_elbow = new double[3];
        double[] L6_wrist = new double[3];

        double l_se_2;
        double l_se;
        double l_ew_2;
        double l_ew;
        double atan_e;

        const double M_PI = Math.PI;
        const double M_PI_2 = Math.PI / 2;

        ArmConfig armConfig = new ArmConfig();

        void MatMultiply(double[] _matrix1, double[] _matrix2, double[] _matrixOut
            , int _m, int _l, int _n)
        {
            double tmp;
            int i, j, k;
            for (i = 0; i < _m; i++)
            {
                for (j = 0; j < _n; j++)
                {
                    tmp = 0.0f;
                    for (k = 0; k < _l; k++)
                    {
                        tmp += _matrix1[_l * i + k] * _matrix2[_n * k + j];
                    }
                    _matrixOut[_n * i + j] = tmp;
                }
            }
        }

        void RotMatToEulerAngle(double[] _rotationM, double[] _eulerAngles)
        {
            double A, B, C, cb;

            if (Math.Abs(_rotationM[6]) >= 1.0 - 0.0001)
            {
                if (_rotationM[6] < 0)
                {
                    A = 0.0f;
                    B = Math.PI / 2;
                    C = Math.Atan2(_rotationM[1], _rotationM[4]);
                }
                else
                {
                    A = 0.0f;
                    B = -Math.PI / 2;
                    C = -Math.Atan2(_rotationM[1], _rotationM[4]);
                }
            }
            else
            {
                B = Math.Atan2(-_rotationM[6], Math.Sqrt(_rotationM[0] * _rotationM[0] + _rotationM[3] * _rotationM[3]));
                cb = Math.Cos(B);
                A = Math.Atan2(_rotationM[3] / cb, _rotationM[0] / cb);
                C = Math.Atan2(_rotationM[7] / cb, _rotationM[8] / cb);
            }

            _eulerAngles[3] = C;
            _eulerAngles[4] = B;
            _eulerAngles[5] = A;
        }

        void EulerAngleToRotMat(double[] _eulerAngles, double[] _rotationM)
        {
            double ca, cb, cc, sa, sb, sc;

            cc = Math.Cos(_eulerAngles[0]);
            cb = Math.Cos(_eulerAngles[1]);
            ca = Math.Cos(_eulerAngles[2]);
            sc = Math.Sin(_eulerAngles[0]);
            sb = Math.Sin(_eulerAngles[1]);
            sa = Math.Sin(_eulerAngles[2]);

            _rotationM[0] = ca * cb;
            _rotationM[1] = ca * sb * sc - sa * cc;
            _rotationM[2] = ca * sb * cc + sa * sc;
            _rotationM[3] = sa * cb;
            _rotationM[4] = sa * sb * sc + ca * cc;
            _rotationM[5] = sa * sb * cc - ca * sc;
            _rotationM[6] = -sb;
            _rotationM[7] = cb * sc;
            _rotationM[8] = cb * cc;
        }

        public Dof6kinematic(ArmConfig armConfig)
        {
            this.armConfig = armConfig;

            double[,] tmp_DH_matrix = new double[6, 4]{
                { 0.0f,       armConfig.L_BASE,      armConfig.D_BASE,  -Math.PI/2},
                { -Math.PI/2, 0.0f,                  armConfig.L_ARM,   0.0f},
                { Math.PI/2,  armConfig.D_ELBOW,     0.0f,              Math.PI/2},
                { 0.0f,       armConfig.L_FOREARM,   0.0f,              -Math.PI/2},
                { 0.0f,       0.0f,                  0.0f,              Math.PI/2},
                { 0.0f,       armConfig.L_WRIST,     0.0f,              0.0f}
            };
            DH_matrix = tmp_DH_matrix;

            L1_base = new double[] { armConfig.D_BASE, -armConfig.L_BASE, 0.0f };
            L2_arm = new double[] { armConfig.L_ARM, 0.0f, 0.0f };
            L3_elbow = new double[] { -armConfig.D_ELBOW, 0.0f, armConfig.L_FOREARM };
            L6_wrist = new double[] { 0.0f, 0.0f, armConfig.L_WRIST };

            l_se_2 = armConfig.L_ARM * armConfig.L_ARM;
            l_se = armConfig.L_ARM;
            l_ew_2 = armConfig.L_FOREARM * armConfig.L_FOREARM + armConfig.D_ELBOW * armConfig.D_ELBOW;
            l_ew = 0;
            atan_e = 0;


        }
        //正解
        public bool SolveFK(Joint6D_t joint, Pose6D_t pose)
        {
            double[] q_in = new double[6];
            double[] q = new double[6];
            double cosq, sinq;
            double cosa, sina;
            double[] P06 = new double[6];
            double[] R06 = new double[9];
            double[][] R = new double[6][] { new double[9], new double[9], new double[9], new double[9], new double[9], new double[9] };
            double[] R02 = new double[9];
            double[] R03 = new double[9];
            double[] R04 = new double[9];
            double[] R05 = new double[9];
            double[] L0_bs = new double[3];
            double[] L0_se = new double[3];
            double[] L0_ew = new double[3];
            double[] L0_wt = new double[3];

            for (int i = 0; i < 6; i++)
                q_in[i] = joint.a[i] / RAD_TO_DEG;

            for (int i = 0; i < 6; i++)
            {
                q[i] = q_in[i] + DH_matrix[i, 0];
                cosq = Math.Cos(q[i]);
                sinq = Math.Sin(q[i]);
                cosa = Math.Cos(DH_matrix[i, 3]);
                sina = Math.Sin(DH_matrix[i, 3]);

                R[i][0] = cosq;
                R[i][1] = -cosa * sinq;
                R[i][2] = sina * sinq;
                R[i][3] = sinq;
                R[i][4] = cosa * cosq;
                R[i][5] = -sina * cosq;
                R[i][6] = 0.0f;
                R[i][7] = sina;
                R[i][8] = cosa;
            }
            MatMultiply(R[0], R[1], R02, 3, 3, 3);
            MatMultiply(R02, R[2], R03, 3, 3, 3);
            MatMultiply(R03, R[3], R04, 3, 3, 3);
            MatMultiply(R04, R[4], R05, 3, 3, 3);
            MatMultiply(R05, R[5], R06, 3, 3, 3);

            MatMultiply(R[0], L1_base, L0_bs, 3, 3, 1);
            MatMultiply(R02, L2_arm, L0_se, 3, 3, 1);
            MatMultiply(R03, L3_elbow, L0_ew, 3, 3, 1);
            MatMultiply(R06, L6_wrist, L0_wt, 3, 3, 1);

            for (int i = 0; i < 3; i++)
                P06[i] = L0_bs[i] + L0_se[i] + L0_ew[i] + L0_wt[i];

            RotMatToEulerAngle(R06, P06);

            pose.X = (float)P06[0];
            pose.Y = (float)P06[1];
            pose.Z = (float)P06[2];
            pose.A = (float)(P06[3] * RAD_TO_DEG);
            pose.B = (float)(P06[4] * RAD_TO_DEG);
            pose.C = (float)(P06[5] * RAD_TO_DEG);

            //pose.A = (float)(P06[3]);
            //pose.B = (float)(P06[4]);
            //pose.C = (float)(P06[5]);
            pose.R = R06.ToArray();

            return true;
        }
        //逆解
        public bool SolveIK(Pose6D_t _inputPose6D, Joint6D_t _lastJoint6D, out IKSolves_t _outputSolves)
        {
            _outputSolves = new IKSolves_t();

            double[] qs = new double[2];
            double[][] qa = new double[2][] { new double[2], new double[2] };
            double[][] qw = new double[2][] { new double[3], new double[3] };
            double cosqs, sinqs;
            double[] cosqa = new double[2], sinqa = new double[2];
            double cosqw, sinqw;
            double[] P06 = new double[6];
            double[] R06 = new double[9];
            double[] P0_w = new double[3];
            double[] P1_w = new double[3];
            double[] L0_wt = new double[3];
            double[] L1_sw = new double[3];
            double[] R10 = new double[9];
            double[] R31 = new double[9];
            double[] R30 = new double[9];
            double[] R36 = new double[9];
            double l_sw_2, l_sw, atan_a, acos_a, acos_e;

            int ind_arm, ind_elbow, ind_wrist;
            int i;

            if (0 == l_ew)
            {
                l_ew = Math.Sqrt(l_ew_2);
                atan_e = Math.Atan(armConfig.D_ELBOW / armConfig.L_FOREARM);
            }

            P06[0] = _inputPose6D.X;
            P06[1] = _inputPose6D.Y;
            P06[2] = _inputPose6D.Z;
            if (!_inputPose6D.hasR)
            {
                P06[3] = _inputPose6D.A / RAD_TO_DEG;
                P06[4] = _inputPose6D.B / RAD_TO_DEG;
                P06[5] = _inputPose6D.C / RAD_TO_DEG;
                EulerAngleToRotMat(P06.Skip(3).ToArray(), R06);
            }
            else
            {
                Array.Copy(R06, _inputPose6D.R, 9);
            }
            for (i = 0; i < 2; i++)
            {
                qs[i] = _lastJoint6D.a[0];
                qa[i][0] = _lastJoint6D.a[1];
                qa[i][1] = _lastJoint6D.a[2];
                qw[i][0] = _lastJoint6D.a[3];
                qw[i][1] = _lastJoint6D.a[4];
                qw[i][2] = _lastJoint6D.a[5];
            }
            MatMultiply(R06, L6_wrist, L0_wt, 3, 3, 1);
            for (i = 0; i < 3; i++)
            {
                P0_w[i] = P06[i] - L0_wt[i];
            }
            if (Math.Sqrt(P0_w[0] * P0_w[0] + P0_w[1] * P0_w[1]) <= 0.000001)
            {
                qs[0] = _lastJoint6D.a[0];
                qs[1] = _lastJoint6D.a[0];
                for (i = 0; i < 4; i++)
                {
                    _outputSolves.solFlag[0 + i][0] = -1;
                    _outputSolves.solFlag[4 + i][0] = -1;
                }
            }
            else
            {
                qs[0] = Math.Atan2(P0_w[1], P0_w[0]);
                qs[1] = Math.Atan2(-P0_w[1], -P0_w[0]);
                for (i = 0; i < 4; i++)
                {
                    _outputSolves.solFlag[0 + i][0] = 1;
                    _outputSolves.solFlag[4 + i][0] = 1;
                }
            }
            for (ind_arm = 0; ind_arm < 2; ind_arm++)
            {
                cosqs = Math.Cos(qs[ind_arm] + DH_matrix[0, 0]);
                sinqs = Math.Sin(qs[ind_arm] + DH_matrix[0, 0]);

                R10[0] = cosqs;
                R10[1] = sinqs;
                R10[2] = 0.0f;
                R10[3] = 0.0f;
                R10[4] = 0.0f;
                R10[5] = -1.0f;
                R10[6] = -sinqs;
                R10[7] = cosqs;
                R10[8] = 0.0f;

                MatMultiply(R10, P0_w, P1_w, 3, 3, 1);
                for (i = 0; i < 3; i++)
                {
                    L1_sw[i] = P1_w[i] - L1_base[i];
                }
                l_sw_2 = L1_sw[0] * L1_sw[0] + L1_sw[1] * L1_sw[1];
                l_sw = Math.Sqrt(l_sw_2);

                if (Math.Abs(l_se + l_ew - l_sw) <= 0.000001)
                {
                    qa[0][0] = Math.Atan2(L1_sw[1], L1_sw[0]);
                    qa[1][0] = qa[0][0];
                    qa[0][1] = 0.0f;
                    qa[1][1] = 0.0f;
                    if (l_sw > l_se + l_ew)
                    {
                        for (i = 0; i < 2; i++)
                        {
                            _outputSolves.solFlag[4 * ind_arm + 0 + i][1] = 0;
                            _outputSolves.solFlag[4 * ind_arm + 2 + i][1] = 0;
                        }
                    }
                    else
                    {
                        for (i = 0; i < 2; i++)
                        {
                            _outputSolves.solFlag[4 * ind_arm + 0 + i][1] = 1;
                            _outputSolves.solFlag[4 * ind_arm + 2 + i][1] = 1;
                        }
                    }
                }
                else if (Math.Abs(l_sw - Math.Abs(l_se - l_ew)) <= 0.000001)
                {
                    qa[0][0] = Math.Atan2(L1_sw[1], L1_sw[0]);
                    qa[1][0] = qa[0][0];
                    if (0 == ind_arm)
                    {
                        qa[0][1] = M_PI;
                        qa[1][1] = -M_PI;
                    }
                    else
                    {
                        qa[0][1] = -M_PI;
                        qa[1][1] = M_PI;
                    }
                    if (l_sw < Math.Abs(l_se - l_ew))
                    {
                        for (i = 0; i < 2; i++)
                        {
                            _outputSolves.solFlag[4 * ind_arm + 0 + i][1] = 0;
                            _outputSolves.solFlag[4 * ind_arm + 2 + i][1] = 0;
                        }
                    }
                    else
                    {
                        for (i = 0; i < 2; i++)
                        {
                            _outputSolves.solFlag[4 * ind_arm + 0 + i][1] = 1;
                            _outputSolves.solFlag[4 * ind_arm + 2 + i][1] = 1;
                        }
                    }
                }
                else
                {
                    atan_a = Math.Atan2(L1_sw[1], L1_sw[0]);
                    acos_a = 0.5f * (l_se_2 + l_sw_2 - l_ew_2) / (l_se * l_sw);
                    if (acos_a >= 1.0f) acos_a = 0.0f;
                    else if (acos_a <= -1.0f) acos_a = M_PI;
                    else acos_a = Math.Acos(acos_a);
                    acos_e = 0.5f * (l_se_2 + l_ew_2 - l_sw_2) / (l_se * l_ew);
                    if (acos_e >= 1.0f) acos_e = 0.0f;
                    else if (acos_e <= -1.0f) acos_e = M_PI;
                    else acos_e = Math.Acos(acos_e);
                    if (0 == ind_arm)
                    {
                        qa[0][0] = atan_a - acos_a + M_PI_2;
                        qa[0][1] = atan_e - acos_e + M_PI;
                        qa[1][0] = atan_a + acos_a + M_PI_2;
                        qa[1][1] = atan_e + acos_e - M_PI;

                    }
                    else
                    {
                        qa[0][0] = atan_a + acos_a + M_PI_2;
                        qa[0][1] = atan_e + acos_e - M_PI;
                        qa[1][0] = atan_a - acos_a + M_PI_2;
                        qa[1][1] = atan_e - acos_e + M_PI;
                    }
                    for (i = 0; i < 2; i++)
                    {
                        _outputSolves.solFlag[4 * ind_arm + 0 + i][1] = 1;
                        _outputSolves.solFlag[4 * ind_arm + 2 + i][1] = 1;
                    }
                }
                for (ind_elbow = 0; ind_elbow < 2; ind_elbow++)
                {
                    cosqa[0] = Math.Cos(qa[ind_elbow][0] + DH_matrix[1, 0]);
                    sinqa[0] = Math.Sin(qa[ind_elbow][0] + DH_matrix[1, 0]);
                    cosqa[1] = Math.Cos(qa[ind_elbow][1] + DH_matrix[2, 0]);
                    sinqa[1] = Math.Sin(qa[ind_elbow][1] + DH_matrix[2, 0]);

                    R31[0] = cosqa[0] * cosqa[1] - sinqa[0] * sinqa[1];
                    R31[1] = cosqa[0] * sinqa[1] + sinqa[0] * cosqa[1];
                    R31[2] = 0.0f;
                    R31[3] = 0.0f;
                    R31[4] = 0.0f;
                    R31[5] = 1.0f;
                    R31[6] = cosqa[0] * sinqa[1] + sinqa[0] * cosqa[1];
                    R31[7] = -cosqa[0] * cosqa[1] + sinqa[0] * sinqa[1];
                    R31[8] = 0.0f;

                    MatMultiply(R31, R10, R30, 3, 3, 3);
                    MatMultiply(R30, R06, R36, 3, 3, 3);

                    if (R36[8] >= 1.0 - 0.000001)
                    {
                        cosqw = 1.0f;
                        qw[0][1] = 0.0f;
                        qw[1][1] = 0.0f;
                    }
                    else if (R36[8] <= -1.0 + 0.000001)
                    {
                        cosqw = -1.0f;
                        if (0 == ind_arm)
                        {
                            qw[0][1] = M_PI;
                            qw[1][1] = -M_PI;
                        }
                        else
                        {
                            qw[0][1] = -M_PI;
                            qw[1][1] = M_PI;
                        }
                    }
                    else
                    {
                        cosqw = R36[8];
                        if (0 == ind_arm)
                        {
                            qw[0][1] = Math.Acos(cosqw);
                            qw[1][1] = -Math.Acos(cosqw);
                        }
                        else
                        {
                            qw[0][1] = -Math.Acos(cosqw);
                            qw[1][1] = Math.Acos(cosqw);
                        }
                    }
                    if (1.0f == cosqw || -1.0f == cosqw)
                    {
                        if (0 == ind_arm)
                        {
                            qw[0][0] = _lastJoint6D.a[3];
                            cosqw = Math.Cos(_lastJoint6D.a[3] + DH_matrix[3, 0]);
                            sinqw = Math.Sin(_lastJoint6D.a[3] + DH_matrix[3, 0]);
                            qw[0][2] = Math.Atan2(cosqw * R36[3] - sinqw * R36[0], cosqw * R36[0] + sinqw * R36[3]);
                            qw[1][2] = _lastJoint6D.a[5];
                            cosqw = Math.Cos(_lastJoint6D.a[5] + DH_matrix[5, 0]);
                            sinqw = Math.Sin(_lastJoint6D.a[5] + DH_matrix[5, 0]);
                            qw[1][0] = Math.Atan2(cosqw * R36[3] - sinqw * R36[0], cosqw * R36[0] + sinqw * R36[3]);
                        }
                        else
                        {
                            qw[0][2] = _lastJoint6D.a[5];
                            cosqw = Math.Cos(_lastJoint6D.a[5] + DH_matrix[5, 0]);
                            sinqw = Math.Sin(_lastJoint6D.a[5] + DH_matrix[5, 0]);
                            qw[0][0] = Math.Atan2(cosqw * R36[3] - sinqw * R36[0], cosqw * R36[0] + sinqw * R36[3]);
                            qw[1][0] = _lastJoint6D.a[3];
                            cosqw = Math.Cos(_lastJoint6D.a[3] + DH_matrix[3, 0]);
                            sinqw = Math.Sin(_lastJoint6D.a[3] + DH_matrix[3, 0]);
                            qw[1][2] = Math.Atan2(cosqw * R36[3] - sinqw * R36[0], cosqw * R36[0] + sinqw * R36[3]);
                        }
                        _outputSolves.solFlag[4 * ind_arm + 2 * ind_elbow + 0][2] = -1;
                        _outputSolves.solFlag[4 * ind_arm + 2 * ind_elbow + 1][2] = -1;
                    }
                    else
                    {
                        if (0 == ind_arm)
                        {
                            qw[0][0] = Math.Atan2(R36[5], R36[2]);
                            qw[1][0] = Math.Atan2(-R36[5], -R36[2]);
                            qw[0][2] = Math.Atan2(R36[7], -R36[6]);
                            qw[1][2] = Math.Atan2(-R36[7], R36[6]);
                        }
                        else
                        {
                            qw[0][0] = Math.Atan2(-R36[5], -R36[2]);
                            qw[1][0] = Math.Atan2(R36[5], R36[2]);
                            qw[0][2] = Math.Atan2(-R36[7], R36[6]);
                            qw[1][2] = Math.Atan2(R36[7], -R36[6]);
                        }
                        _outputSolves.solFlag[4 * ind_arm + 2 * ind_elbow + 0][2] = 1;
                        _outputSolves.solFlag[4 * ind_arm + 2 * ind_elbow + 1][2] = 1;
                    }
                    for (ind_wrist = 0; ind_wrist < 2; ind_wrist++)
                    {
                        if (qs[ind_arm] > M_PI)
                            _outputSolves.config[4 * ind_arm + 2 * ind_elbow + ind_wrist].a[0] =
                                qs[ind_arm] - M_PI;
                        else if (qs[ind_arm] < -M_PI)
                            _outputSolves.config[4 * ind_arm + 2 * ind_elbow + ind_wrist].a[0] =
                                qs[ind_arm] + M_PI;
                        else
                            _outputSolves.config[4 * ind_arm + 2 * ind_elbow + ind_wrist].a[0] = qs[ind_arm];

                        for (i = 0; i < 2; i++)
                        {
                            if (qa[ind_elbow][i] > M_PI)
                                _outputSolves.config[4 * ind_arm + 2 * ind_elbow + ind_wrist].a[1 + i] =
                                    qa[ind_elbow][i] - M_PI;
                            else if (qa[ind_elbow][i] < -M_PI)
                                _outputSolves.config[4 * ind_arm + 2 * ind_elbow + ind_wrist].a[1 + i] =
                                    qa[ind_elbow][i] + M_PI;
                            else
                                _outputSolves.config[4 * ind_arm + 2 * ind_elbow + ind_wrist].a[1 + i] =
                                    qa[ind_elbow][i];
                        }

                        for (i = 0; i < 3; i++)
                        {
                            if (qw[ind_wrist][i] > M_PI)
                                _outputSolves.config[4 * ind_arm + 2 * ind_elbow + ind_wrist].a[3 + i] =
                                    qw[ind_wrist][i] - M_PI;
                            else if (qw[ind_wrist][i] < -M_PI)
                                _outputSolves.config[4 * ind_arm + 2 * ind_elbow + ind_wrist].a[3 + i] =
                                    qw[ind_wrist][i] + M_PI;
                            else
                                _outputSolves.config[4 * ind_arm + 2 * ind_elbow + ind_wrist].a[3 + i] =
                                    qw[ind_wrist][i];
                        }
                    }
                }
            }

            foreach (var one in _outputSolves.config)
            {
                for (int j = 0; j < one.a.Length; j++)
                {
                    one.a[j] = one.a[j] * RAD_TO_DEG;
                }
            }

            return true;
        }
    }
}
