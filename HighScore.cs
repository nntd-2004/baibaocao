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
    public partial class HighScore : Form
    {
        public HighScore()
        {
            InitializeComponent();
        }

        private void HighScore_Load(object sender, EventArgs e)
        {
            var history = Utils.ReadFileHighScore();
            foreach (var item in history)
            {
                lstHightScore.Items.Add(string.Format("{0}  {1} {2}",item.Id,item.Name,item.Score));
            }

        }

        private void lstHightScore_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
