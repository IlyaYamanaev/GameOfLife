using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel; //for close
using GameOfLife;
using System.Media;
using GameOfLife.Properties;
using System.Runtime.Remoting;

namespace GameOfLife
{
    public class Settings : Control
    {        
        Сolony game;
        public Timer timer;
        Game form;
        TextBox conditionOfBirth, conditionOfDeath, w, h, kom;
        Label conditionOfBirthText, conditionOfDeathText, sizeLabel, xText, typesWorldText, speedLabel, value_milsec;
        ListBox typesWorldList;
        Button clearButton, nextButton; 
        public Button startButton;
        TrackBar speedBar;
        
        public Settings(int x, int y, Сolony game, Game form) : base("Settings", x, y, 340, 310)
        {
            this.game = game;
            this.form = form;
            SuspendLayout();
            conditionOfBirth = new TextBox();
            conditionOfDeath = new TextBox();
            w = new TextBox();
            h = new TextBox();
            conditionOfBirthText = new Label();
            conditionOfDeathText = new Label();
            sizeLabel = new Label();
            xText = new Label();
            value_milsec = new Label();
            typesWorldText = new Label();
            speedLabel = new Label();
            typesWorldList = new ListBox();
            nextButton = new Button();
            startButton = new Button();
            clearButton = new Button();
            speedBar = new TrackBar();
            Paint += new PaintEventHandler(OnPaint);


            conditionOfBirthText.Text = "Соседей для\n    рождения:";
            conditionOfDeathText.Text = "Умирает\n         при:";
            sizeLabel.Text = "SIZE:";
            xText.Text = "x";
            typesWorldText.Text = "Тип границ мира:";
            value_milsec.Text = "1 ген.\\сек";
            speedLabel.Text = "Скорость:";
            nextButton.Text = "Next";
            startButton.Text = "Start";
            clearButton.Text = "CLEAR";
            conditionOfBirth.Text = "e";
            conditionOfDeath.Text = "e";
            w.Text = "15";
            h.Text = "15";



            conditionOfBirthText.Location = new Point(7, 10);
            conditionOfBirth.Location = new Point(80, 15);
            conditionOfDeathText.Location = new Point(172, 10);
            conditionOfDeath.Location = new Point(227, 15);
            sizeLabel.Location = new Point(130, 150);
            w.Location = new Point(40, 170);
            xText.Location = new Point(144, 172);
            h.Location = new Point(160, 170);
            typesWorldText.Location = new Point(80, 50);
            typesWorldList.Location = new Point(50, 70);
            speedLabel.Location = new Point(100, 210);
            speedBar.Location = new Point(20, 230);
            value_milsec.Location = new Point(230, 230);
            nextButton.Location = new Point(10, 280);
            startButton.Location = new Point(100, 280);
            clearButton.Location = new Point(250, 280);
            

            conditionOfBirth.Width = 80;
            conditionOfDeath.Width = 80;
            typesWorldList.Size = new System.Drawing.Size(200, 70);
            conditionOfBirthText.AutoSize = true;
            conditionOfDeathText.AutoSize = true;
            sizeLabel.AutoSize = true;
            xText.AutoSize = true;
            typesWorldText.AutoSize = true;
            speedLabel.AutoSize = true;
            speedBar.Size = new System.Drawing.Size(200, 30);
            speedBar.Minimum = 1;
            speedBar.Maximum = 20;
            speedBar.Value = 1;
            speedBar.TickStyle = TickStyle.TopLeft;
            value_milsec.AutoSize = true;
            typesWorldList.Items.Add("Граница - портал");
            typesWorldList.Items.Add("Граница - барьер");
            typesWorldList.Items.Add("Бесконечный мир");
            


            speedBar.Scroll += new EventHandler(this.changeSpeed);
            conditionOfBirth.TextChanged += new EventHandler(this.SetRules);
            conditionOfDeath.TextChanged += new EventHandler(this.SetRules);
            w.TextChanged += new EventHandler(this.SetSize);
            h.TextChanged += new EventHandler(this.SetSize);
            typesWorldList.SelectedIndexChanged += new EventHandler(this.SetType);
            nextButton.Click += new EventHandler(this.NextClick);
            startButton.Click += new EventHandler(this.StartClick);
            clearButton.Click += new EventHandler(this.ClearClick);

            Controls.Add(conditionOfBirthText);
            Controls.Add(conditionOfBirth);
            Controls.Add(conditionOfDeathText);
            Controls.Add(conditionOfDeath);
            Controls.Add(sizeLabel);
            Controls.Add(w);
            Controls.Add(xText);
            Controls.Add(h);
            Controls.Add(typesWorldText);
            Controls.Add(typesWorldList);
            Controls.Add(speedLabel);
            Controls.Add(speedBar);
            Controls.Add(value_milsec);
            Controls.Add(nextButton);
            Controls.Add(startButton);
            Controls.Add(clearButton);
            Controls.Add(kom);


            ResumeLayout(false);

            timer = new Timer();
            timer.Tick += new EventHandler(NextClick);
            timer.Interval = Speed;
            while (startButton.Text == "Stop")
            {
                Application.DoEvents();
            }
        }

        public void  OnPaint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder3D(e.Graphics, e.ClipRectangle, Border3DStyle.Bump);
        }
        public int   Speed
        {
            get
            {
                return 1000/speedBar.Value;
            }
        }
        private void changeSpeed(object sender, System.EventArgs e)
        {
            value_milsec.Text = speedBar.Value + " ген.\\" + "сек";
            timer.Interval = Speed;
            Invalidate();
        }
        private void SetRules(object sender, System.EventArgs e)
        {
            int buff;
            if (int.TryParse(conditionOfBirth.Text, out buff) && int.TryParse(conditionOfDeath.Text, out buff)) game.SetRules(conditionOfBirth.Text, conditionOfDeath.Text);

            conditionOfBirth.Text = game.GetB;
            conditionOfDeath.Text = game.GetS;
        }
        private void SetSize(object sender, System.EventArgs e)
        {
            int _;
            if (int.TryParse(w.Text, out _) && _ > 0 && int.TryParse(h.Text, out _) && _ > 0) game.SetSize(int.Parse(w.Text), int.Parse(h.Text));
            w.Text = game.Weight.ToString();
            h.Text = game.Hight.ToString();
            form.Invalidate();
        }
        private void SetType(object sender, System.EventArgs e)
        {
            if (typesWorldList.SelectedIndex == 2)
            {
                game.SetSize(Int32.MaxValue / 10, Int32.MaxValue / 10);
                w.Hide();
                h.Hide();
                sizeLabel.Hide();
                xText.Hide();
                form.offset = new Point(Int32.MaxValue / 200 - 1 + form.offset.X % form.scale, Int32.MaxValue / 200 - 1 + form.offset.Y % form.scale);
                game.Clear();
            }
            else if (game.Type == 2)
            {
                w.Show();
                h.Show();
                sizeLabel.Show();
                xText.Show();
                w.Text = "0" + w.Text;
                h.Text = h.Text;
                form.offset = new Point(-(form.ClientSize.Width / 2 - form.scale * game.Weight / 2), -(form.ClientSize.Height / 2 - form.scale * game.Hight / 2));
                game.Clear();
            }
            game.Type = typesWorldList.SelectedIndex;
            form.Invalidate();
            Invalidate();
        }
        private void NextClick(object sender, EventArgs e)
        {
            game.NextGen();
            form.Invalidate();
        }
        private void StartClick(object sender, EventArgs e)
        {

            if (startButton.Text == "Start")
            {                
                timer.Start();   
            }
            else
            {
                timer.Stop();                 
            }
            startButton.Text = (startButton.Text == "Start") ? "Stop" : "Start";
        }
        private void ClearClick(object sender, EventArgs e)
        {
            game.Clear();
            form.Invalidate();
        }

    }
    public partial class Game : Form
    {
        private Сolony colony;
        private Settings settings;
        private bool settingsHide;
        private double positionX, positionY;

        public Point offset; 
        public int scale;
        private Point mouseButtonPressedCoorditates; 
        private Point tempPoint; 
        private string mouseButtonWasPressed; 
        
        public Game()
        {
            DoubleBuffered = true;            
            FormBorderStyle = FormBorderStyle.Sizable;
            WindowState = FormWindowState.Maximized;
            BackColor = Color.White;
            colony = new Сolony(15, 15, "3", "23", 0);
            settings = new Settings(0, 0, colony, this);
            Controls.Add(settings);
            settings.Hide();
            settingsHide = true;
            scale = 30;
            offset = new Point(0, 0);
        }


        internal void Draw(Graphics graphics)
        {
            if (scale >= 20) using (Pen pen = new Pen(Color.FromArgb(260 - scale, 260 - scale, 260 - scale), 1.0f))
            {
                for (int i = (scale - offset.X % scale) % scale; i <= ClientSize.Width; i += scale)
                {
                    graphics.DrawLine(pen, i, 0, i, ClientSize.Height);
                }
                for (int i = (scale - offset.Y % scale) % scale; i <= ClientSize.Height; i += scale)
                {
                    graphics.DrawLine(pen, 0, ClientSize.Height - i, ClientSize.Width, ClientSize.Height - i);
                }
            }
            if (colony.Type != 2) using (Pen pen = new Pen((colony.Type == 0) ? Color.Red : Color.Blue, 2.0f))
            {
                graphics.DrawLine(pen, -offset.X + colony.Weight * scale, (ClientSize.Height + offset.Y), 
                    -offset.X + colony.Weight * scale, (ClientSize.Height + offset.Y) - colony.Hight * scale);
                graphics.DrawLine(pen, -offset.X, (ClientSize.Height + offset.Y) - colony.Hight * scale, 
                    -offset.X + colony.Weight * scale, (ClientSize.Height + offset.Y) - colony.Hight * scale);
                graphics.DrawLine(pen, -offset.X, (ClientSize.Height + offset.Y), -offset.X + colony.Weight * scale, 
                    (ClientSize.Height + offset.Y));
                graphics.DrawLine(pen, -offset.X, (ClientSize.Height + offset.Y), -offset.X, 
                    (ClientSize.Height + offset.Y) - colony.Hight * scale);
            }
            foreach (var col in colony.Data)
            {
                foreach (var row in col.Value)
                {
                    if (OnWindow(col.Key, row.Key))
                    {
                        using (Brush brush = new SolidBrush(Color.Black))
                        {
                            graphics.FillRectangle(brush, col.Key * scale - offset.X, 
                                ClientSize.Height - (row.Key * scale - offset.Y) - scale, scale, scale);
                        }
                    }
                }
            }
        }
        private bool OnWindow(int x, int y)
        {
            return offset.X <= x * scale + scale && x * scale <= offset.X + ClientSize.Width 
                && offset.Y <= y * scale + scale && y * scale <= offset.Y + ClientSize.Height;
        }
        protected override void OnPaint(PaintEventArgs args)
        {
            Draw(args.Graphics);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            switch (MouseButtons)
            {
                case MouseButtons.Left:
                    mouseButtonWasPressed = "Left";
                    colony.Set((int)((offset.X + e.X) / scale), 
                        (int)((offset.Y + ClientSize.Height - e.Y) / scale));
                    Invalidate();
                    break;
                case MouseButtons.Right:
                    mouseButtonWasPressed = "Right";
                    break;
                default:
                    break;
            }
            mouseButtonPressedCoorditates = new Point(e.X, ClientSize.Height - e.Y);
            tempPoint = new Point(0, 0);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (mouseButtonWasPressed == "Right" && tempPoint.X == 0 && tempPoint.Y == 0)
            {
                if (!settingsHide)
                {
                    settings.Hide();
                    settingsHide = true;
                }
                else
                {
                    settings.Location = new Point(e.X, e.Y);
                    settings.Show();
                    settingsHide = false;
                }
            }
            mouseButtonWasPressed = "";
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (((int)((offset.X + e.X) / scale) != -1 || 
                ((int)((offset.Y + ClientSize.Height - e.Y) / scale) != -1)) &&
                mouseButtonWasPressed == "Left")
            {
                colony.Set((int)((offset.X + e.X) / scale), (int)((offset.Y + ClientSize.Height - e.Y) / scale));
                Invalidate();
            }
            if (mouseButtonWasPressed == "Right")
            {
                offset.X = offset.X + tempPoint.X;
                offset.Y = offset.Y + tempPoint.Y;
                tempPoint = new Point((e.X - mouseButtonPressedCoorditates.X), 
                            (ClientSize.Height - e.Y) - mouseButtonPressedCoorditates.Y);
                offset.X = offset.X - tempPoint.X;
                offset.Y = offset.Y - tempPoint.Y;
                Invalidate();
            }
            
        }        
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            positionX = ((double)offset.X + e.X) / (double)scale;
            positionY = ((double)offset.Y + ClientSize.Height - e.Y) / (double)scale;
            scale = Math.Max(2, Math.Min(200, scale + e.Delta / 120));
            offset.X = (int)(positionX * scale - e.X);
            offset.Y = (int)(positionY * scale - (ClientSize.Height - e.Y));
            Invalidate();
        }

     



        public static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Game());
            return 0;
        }
    }

}
