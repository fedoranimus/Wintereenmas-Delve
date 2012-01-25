using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.storyTelling
{
	public class StoryTeller
	{
		private List<InstalledVoice> _voices;
		private String _narratorVoice;
		private String _mentorVoice;
		

		private SpeechSynthesizer _synthesizer;

		private Boolean _currentlyPlaying;

		List<Story> _stories;
		Story _currentStory;

		public EventHandler StoryComplete;

		public StoryTeller()
			: base()
		{
			_synthesizer = new SpeechSynthesizer();
			_voices = _synthesizer.GetInstalledVoices().ToList();

			if (_voices.Count <= 0)
			{ System.Diagnostics.Debug.WriteLine("MainWindow() No voices found!"); }
			else
			{
				_narratorVoice = _voices[0].VoiceInfo.Name;
				if (_voices.Count >= 2)
				{ _mentorVoice = _voices[1].VoiceInfo.Name; }
				else
				{ _mentorVoice = _voices[0].VoiceInfo.Name; }
			}

			_stories = new List<Story>();
			_currentlyPlaying = false;

			_synthesizer.SpeakCompleted += OnSpeakCompleted;
		}

		public void TellStory(Story story)
		{
			if (!story.HasNextLine)
			{ return; } // Do nothing

			_stories.Add(story);

			if (!CurrentlyPlaying)
			{
				_currentlyPlaying = true;
				_currentStory = story;

				StoryLine currentLine = _currentStory.NextLine();
				_synthesizer.SelectVoice(currentLine.Voice);
				_synthesizer.SpeakAsync(currentLine.Line);
			}
		}

		public Boolean CurrentlyPlaying
		{
			get { return _currentlyPlaying; }
		}

		public List<String> GetAvailableVoices()
		{
			List<String> availableVoices = new List<String>();
			foreach (InstalledVoice voice in _voices)
			{ availableVoices.Add(voice.VoiceInfo.Name); }
			return availableVoices;
		}

		public String MentorVoice
		{
			get { return _mentorVoice; }
		}

		public String NarratorVoice
		{
			get { return _narratorVoice; }
		}

		private void OnSpeakCompleted(object sender, EventArgs args)
		{
			System.Diagnostics.Debug.WriteLine("OnSpeakCompleted(sender, args)");

			// Get the next line in the current story
			if (_currentStory.HasNextLine)
			{
				StoryLine currentLine = _currentStory.NextLine();
				_synthesizer.SelectVoice(currentLine.Voice);
				_synthesizer.SpeakAsync(currentLine.Line);
			}
			else
			{
				int storyIndex = _stories.IndexOf(_currentStory);
				_stories.RemoveAt(storyIndex);

				// Do we have another Story
				if (_stories.Count > 0)
				{
					_currentStory = _stories[_stories.Count - 1];

					StoryLine currentLine = _currentStory.NextLine();
					_synthesizer.SelectVoice(currentLine.Voice);
					_synthesizer.SpeakAsync(currentLine.Line);
				}
				else
				{
					_currentStory = null;
					_currentlyPlaying = false;

					if (StoryComplete != null)
					{ StoryComplete(this, new EventArgs()); }
				}
			}
		}
	}
}
