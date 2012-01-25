using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.storyTelling
{
	public class Story
	{
		List<StoryLine> _lines;

		public Story()
			: base()
		{
			_lines = new List<StoryLine>();
		}

		public void Add(String voice, String line)
		{
			StoryLine narrationLine = new StoryLine() { Voice = voice, Line = line };
			_lines.Add(narrationLine);
		}

		public Boolean HasNextLine
		{
			get { return (_lines.Count > 0); }
		}

		public StoryLine NextLine()
		{
			if (_lines.Count <= 0)
			{ return null; }

			StoryLine line = _lines[0];
			_lines.RemoveAt(0);
			return line;
		}
	}
}
