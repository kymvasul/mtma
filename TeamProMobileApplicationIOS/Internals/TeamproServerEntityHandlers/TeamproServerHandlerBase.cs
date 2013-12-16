using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace TeamProMobileApplicationIOS
{
	abstract class TeamproServerHandlerBase<T> : IDisposable
	{
		private readonly XmlReader _reader;
		protected class TeamproServerHandlerProperty {
			public string Value { get; set;}
			public Queue<string> Path { get; set;}
			public int QueDepth { get; private set; }

			public TeamproServerHandlerProperty(params string[] path)
			{
				if (path != null && path.Any())
				{
					Path = new Queue<string>(path);

					if (string.IsNullOrEmpty(path[0]))
					{
						Path.Dequeue();
						QueDepth = -1;
					}
					else
						QueDepth = path.Count();
				}
			}

			public void SetQueueDepth(int depth)
			{
				QueDepth = depth + Path.Count ();
			}
		}

		public TeamproServerHandlerBase(Stream stream)
		{
			_reader = XmlReader.Create (stream);
		}

		protected Stack<string> CallStack = new Stack<string> ();
		protected abstract void StartElementImpl (string uri, string localName, string qName, Dictionary<string, string> attributes);
		protected abstract void ValueImpl (string strValue);
		protected abstract void EndElementImpl (string uri, string localName, string qName);

		public abstract T GetResults ();

		protected bool HandleElement(string qName, TeamproServerHandlerProperty property)
		{
			bool result = false;
			Queue<string> path = property.Path;
			if (path != null && 
			    path.Any() &&
			    (property.QueDepth == -1 || CallStack.Count <= property.QueDepth) &&
			    qName == path.Peek())
			{
				path.Dequeue ();

				if (property.QueDepth == -1) 
				{
					property.SetQueueDepth (CallStack.Count());
				}

				if (!path.Any ())
					result = true;
			}

			return result;
		}

		protected bool HandleElementValue(string strValue, TeamproServerHandlerProperty property)
		{
			Queue<string> propertyPath = property.Path;
			if (property.Value == null &&
				propertyPath != null &&
				!propertyPath.Any () &&
				!string.IsNullOrEmpty (strValue)) 
			{
				property.Value = strValue;
				propertyPath = null;
				return true;
			}

			return false;
		}

		public void Read()
		{
			_reader.MoveToContent ();
			while (_reader.Read()) 
			{
				if (_reader.NodeType == XmlNodeType.Element) 
				{
					CallStack.Push (_reader.Name);
					Dictionary<string, string> attributes = ReadAllAttributes();
					StartElementImpl (_reader.BaseURI, _reader.LocalName, _reader.Name, attributes);
				}

				if (_reader.NodeType == XmlNodeType.EndElement) 
				{
					CallStack.Pop ();
					EndElementImpl (_reader.BaseURI, _reader.LocalName, _reader.Name);
				}

				if (_reader.NodeType == XmlNodeType.Text) 
				{
					ValueImpl (_reader.Value);
				}
			}
		}

		private Dictionary<string, string> ReadAllAttributes()
		{
			int attributesCount = _reader.AttributeCount;
			Dictionary<string, string> result = new Dictionary<string, string> (attributesCount);

			if (attributesCount > 0) 
			{
				for (int i = 0; i < attributesCount; i++) 
				{
					_reader.MoveToAttribute (i);
					result.Add (_reader.Name, _reader.Value);
				}
				_reader.MoveToElement ();
			}

			return result;
		}

		public void Dispose()
		{
			_reader.Dispose ();
		}
	}
}

