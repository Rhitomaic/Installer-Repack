using System;
using System.Windows.Forms;

namespace Creator
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
        }

        public void SetProgress(float progress)
        {
            progressBar.Value = ClampValue((int)(progress * 100), 0, 100);
        }

        public void SetProgress(float initial, int index, int count, float scale)
        {
            float actualProgress = initial + ((index + 1) / count * scale);
            progressBar.Value = ClampValue((int)(actualProgress * 100), 0, 100);
        }

        public int ClampValue(int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        public void SetStatus(string status)
        {
            progressLabel.Text = status;
        }
    }
}
