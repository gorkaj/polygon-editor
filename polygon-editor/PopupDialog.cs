using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace polygon_editor
{
    public static class PopupDialog
    {
        public static string? ShowDialog(string text, string caption, double initValue)
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Width = 150, Text = text };
            TextBox textBox = new TextBox() { Left = 150, Top = 20, Width = 150, Text = initValue.ToString() };
            Button confirmation = new Button() { Text = "Ok", Left = 250, Width = 60, Height = 30, Top = 60, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
        }
    }
}
