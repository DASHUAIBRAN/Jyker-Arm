#if UNITY_2020_3_OR_NEWER
using System;
using System.Collections.Specialized;
using System.Text;
using UnityEngine.Networking;

namespace OpenBLive.Runtime.Utilities
{
    public static class HttpUtility
    {
        private sealed class HttpQsCollection : NameValueCollection
        {
            public override string ToString ()
            {
                int count = Count;
                if (count == 0)
                    return "";
                StringBuilder sb = new StringBuilder ();
                string [] keys = AllKeys;
                for (int i = 0; i < count; i++) {
                    sb.AppendFormat ("{0}={1}&", keys [i], this [keys [i]]);
                }
                if (sb.Length > 0)
                    sb.Length--;
                return sb.ToString ();
            }
        }
        public static NameValueCollection ParseQueryString(string query)
        {
            return ParseQueryString (query, Encoding.UTF8);
        }

        private static NameValueCollection ParseQueryString (string query, Encoding encoding)
        {
            if (query == null)
                throw new ArgumentNullException ("query");
            if (encoding == null)
                throw new ArgumentNullException ("encoding");
            if (query.Length == 0 || (query.Length == 1 && query[0] == '?'))
                return new HttpQsCollection ();
            if (query[0] == '?')
                query = query.Substring (1);
				
            NameValueCollection result = new HttpQsCollection ();
            ParseQueryString (query, encoding, result);
            return result;
        }

        private static void ParseQueryString(string query, Encoding encoding, NameValueCollection result)
        {
            if (query.Length == 0)
                return;

            var decodedLength = query.Length;
            var namePos = 0;
            var first = true;

            while (namePos <= decodedLength)
            {
                int valuePos = -1, valueEnd = -1;
                for (var q = namePos; q < decodedLength; q++)
                {
                    if ((valuePos == -1) && (query[q] == '='))
                    {
                        valuePos = q + 1;
                    }
                    else if (query[q] == '&')
                    {
                        valueEnd = q;
                        break;
                    }
                }

                if (first)
                {
                    first = false;
                    if (query[namePos] == '?')
                        namePos++;
                }

                string name;
                if (valuePos == -1)
                {
                    name = null;
                    valuePos = namePos;
                }
                else
                {
                    name = UnityWebRequest.UnEscapeURL(query.Substring(namePos, valuePos - namePos - 1), encoding);
                }
                if (valueEnd < 0)
                {
                    namePos = -1;
                    valueEnd = query.Length;
                }
                else
                {
                    namePos = valueEnd + 1;
                }
                var value = UnityWebRequest.UnEscapeURL(query.Substring(valuePos, valueEnd - valuePos), encoding);

                result.Add(name, value);
                if (namePos == -1)
                    break;
            }
        }
    }
}
#endif