using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGames
{
    public partial class InputNameHighScore : Form
    {
        public List<HighCoreModel> _highCoreModels;
        public int _score;
        public InputNameHighScore(List<HighCoreModel> highCoreModels, int score)
        {
            InitializeComponent();
            this._highCoreModels = highCoreModels;
            this._score = score;
        }

        private void InputNameHighScore_FormClosed(object sender, FormClosedEventArgs e)
        {
            // check is high score
            var highCore = _highCoreModels.Where(i => i.Score <= _score).FirstOrDefault();
            if (highCore != null && _highCoreModels.Count >= 10)
            {
                var index = _highCoreModels.IndexOf(highCore);
                if (index != -1)
                {
                    _highCoreModels[index].Score = _score;
                    _highCoreModels[index].Name = string.IsNullOrWhiteSpace(txtNameHighScore.Text) ? "Anonymous" : txtNameHighScore.Text;
                }
            }
            else
            {
                _highCoreModels.Add(new HighCoreModel()
                {
                    Score = _score,
                    Name = string.IsNullOrWhiteSpace(txtNameHighScore.Text) ? "Anonymous" : txtNameHighScore.Text
                });
            }

            Utils.WriteFileHighScore(_highCoreModels.OrderByDescending(x=>x.Score).ToList());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
