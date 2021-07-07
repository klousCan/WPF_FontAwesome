using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FontAwesome
{
    public class CssParse
    {
        private string m_source;
        private int m_idx;


        public static bool IsWhiteSpace(char ch)
        {
            return ("\t\n\r ".IndexOf(ch) != -1);
        }

        public void EatWhiteSpace()
        {
            while (!Eof())
            {
                if (!IsWhiteSpace(GetCurrentChar()))
                    return;
                m_idx++;
            }
        }

        public bool Eof()
        {
            return (m_idx >= m_source.Length);
        }

        public string ParseElementName()
        {
            StringBuilder element = new StringBuilder();
            EatWhiteSpace();
            while (!Eof())
            {
                if (GetCurrentChar() == '{')
                {
                    m_idx++;
                    break;
                }
                element.Append(GetCurrentChar());
                m_idx++;
            }

            EatWhiteSpace();
            return element.ToString().Trim();
        }

        public string ParseAttributeName()
        {
            StringBuilder attribute = new StringBuilder();
            EatWhiteSpace();

            while (!Eof())
            {
                if (GetCurrentChar() == ':')
                {
                    m_idx++;
                    break;
                }
                attribute.Append(GetCurrentChar());
                m_idx++;
            }

            EatWhiteSpace();
            return attribute.ToString().Trim();
        }

        public string ParseAttributeValue()
        {
            StringBuilder attribute = new StringBuilder();
            EatWhiteSpace();
            while (!Eof())
            {
                if (GetCurrentChar() == ';')
                {
                    m_idx++;
                    break;
                }
                attribute.Append(GetCurrentChar());
                m_idx++;
            }

            EatWhiteSpace();
            return attribute.ToString().Trim();
        }

        public char GetCurrentChar()
        {
            return GetCurrentChar(0);
        }

        public char GetCurrentChar(int peek)
        {
            if ((m_idx + peek) < m_source.Length)
                return m_source[m_idx + peek];
            else
                return (char)0;
        }

        public char AdvanceCurrentChar()
        {
            return m_source[m_idx++];
        }

        public void Advance()
        {
            m_idx++;
        }

        public string Source
        {
            get
            {
                return m_source;
            }

            set
            {
                m_source = value;
            }
        }

        public List<CssElement> Parse()
        {
            List<CssElement> elements = new List<CssElement>();

            while (!Eof())
            {
                string elementName = ParseElementName();

                if (elementName == null)
                    break;

                CssElement element = new CssElement(elementName);

                string name = ParseAttributeName();
                string value = ParseAttributeValue();

                while (!string.IsNullOrWhiteSpace(name))//(name != null && value != null)
                {
                    element.Add(name, value);

                    EatWhiteSpace();

                    if (GetCurrentChar() == '}')
                    {
                        m_idx++;
                        break;
                    }

                    name = ParseAttributeName();
                    value = ParseAttributeValue();
                }

                elements.Add(element);
            }

            return elements;
        }
    }

    public class CssDocument
    {
        private string _Text;
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
            }
        }

        private List<CssElement> _Elements;
        public List<CssElement> Elements
        {
            get
            {
                return _Elements;
            }
            set
            {
                _Elements = value;
            }
        }

        public CssElement this[string name]
        {
            get
            {
                return Elements.FirstOrDefault(e => e.Name.Contains(name));
            }
        }

        public CssDocument()
        {

        }

        public void Load(string text)
        {
            Text = text;
            CssParse parse = new CssParse();
            parse.Source = Regex.Replace(Text, @"/\*.*?\*/", "", RegexOptions.Compiled);
            Elements = parse.Parse();

        }
    }

    public class CssElement
    {
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        private NameValueCollection _Attributes;
        public NameValueCollection Attributes
        {
            get
            {
                return _Attributes;
            }
            set
            {
                _Attributes = value;
            }
        }

        public CssElement(string name)
        {
            this.Name = name;
            Attributes = new NameValueCollection();
        }

        public void Add(string attribute, string value)
        {
            Attributes[attribute] = value;
        }
    }
}
