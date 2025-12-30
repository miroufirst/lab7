using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab7
{
    public class CarModel
    {
        private Brush _brush;
        private Color _color;

        public CarModel(Color color)
        {
            _color = color;
            _brush = new SolidBrush(color);
        }

        public void Draw(Graphics g, int x, int y)
        {
            g.FillRectangle(_brush, x, y, 40, 20);
            g.DrawRectangle(Pens.Black, x, y, 40, 20);
            g.FillRectangle(Brushes.Cyan, x + 25, y + 2, 10, 16);
            g.FillEllipse(Brushes.Black, x + 5, y + 15, 10, 10);
            g.FillEllipse(Brushes.Black, x + 25, y + 15, 10, 10);
        }
    }

    public static class CarFactory
    {
        private static Dictionary<Color, CarModel> _cache = new Dictionary<Color, CarModel>();

        public static CarModel GetModel(Color color)
        {
            if (!_cache.ContainsKey(color))
            {
                _cache[color] = new CarModel(color);
            }
            return _cache[color];
        }

        public static int GetCacheSize()
        {
            return _cache.Count;
        }
    }

    public class Car
    {
        private CarModel _model;
        private int _x, _y;
        private int _speed;

        public Car(CarModel model, int x, int y, int speed)
        {
            _model = model;
            _x = x;
            _y = y;
            _speed = speed;
        }

        public void Move(int limitX)
        {
            _x += _speed;
            if (_x > limitX) _x = -50;
        }

        public void Draw(Graphics g)
        {
            _model.Draw(g, _x, _y);
        }
    }
    public partial class Form1 : Form
    {
        private List<Car> _cars = new List<Car>();
        private Random _rnd = new Random();
        private Color[] _colors = { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void AddRandomCars(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Color color = _colors[_rnd.Next(_colors.Length)];
                CarModel model = CarFactory.GetModel(color);

                int y = _rnd.Next(50, this.ClientSize.Height - 50);
                int speed = _rnd.Next(5, 15);

                _cars.Add(new Car(model, -50, y, speed));
            }
            UpdateStats();
        }

        private void UpdateStats()
        {
            lblStats.Text = $"Cars on road: {_cars.Count}\n" +
                            $"Unique Models in Memory: {CarFactory.GetCacheSize()}";
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var car in _cars)
            {
                car.Move(this.ClientSize.Width);
            }
            this.Invalidate();
        }

        private void btnAddOne_Click(object sender, EventArgs e)
        {
            AddRandomCars(1);
        }

        private void btnAddMany_Click(object sender, EventArgs e)
        {
            AddRandomCars(100);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            for (int y = 50; y < this.Height; y += 40)
            {
                g.DrawLine(Pens.White, 0, y, this.Width, y);
            }

            foreach (var car in _cars)
            {
                car.Draw(g);
            }
        }
    }
}
