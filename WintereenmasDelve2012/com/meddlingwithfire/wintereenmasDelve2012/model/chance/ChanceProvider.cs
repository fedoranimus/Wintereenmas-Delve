using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.chance
{
	public class ChanceProvider
	{
		private List<ChanceSubmission> _submissions;
		private int _currentIndex;

		public ChanceProvider(List<ChanceSubmission> submissions)
			: base()
		{
			_submissions = submissions;
			_currentIndex = 0;
		}

		public ChanceSubmission NextSubmission()
		{
			ChanceSubmission nextSubmission = _submissions[_currentIndex];
			_currentIndex++;
			if (_currentIndex >= _submissions.Count - 1)
			{ _currentIndex = 0; }
			return nextSubmission;
		}
	}
}
