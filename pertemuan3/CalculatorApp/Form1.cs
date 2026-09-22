using System;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class Form1 : Form
    {
        double firstNumber = 0;
        double secondNumber = 0;
        double result = 0;
        string operation = "";

        public Form1()
        {
            InitializeComponent();
        }

        // Menangani semua tombol angka (0-9) dengan satu handler yang sama
        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (txtDisplay.Text == "0")
                txtDisplay.Text = button.Text;
            else
                txtDisplay.Text += button.Text;
        }

        // Menangani semua tombol operator (+, −, ×, ÷)
        private void OperatorButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            firstNumber = double.Parse(txtDisplay.Text);
            operation = button.Text;

            // Tampilkan "angka operator" (mis. "10 +") di label kecil
            // supaya user tahu operasi apa yang sedang aktif.
            lblExpression.Text = firstNumber + " " + operation;

            txtDisplay.Clear();
        }

        // Menghitung hasil ketika tombol "=" ditekan
        private void btnEquals_Click(object sender, EventArgs e)
        {
            try
            {
                secondNumber = double.Parse(txtDisplay.Text);

                switch (operation)
                {
                    case "+":
                        result = firstNumber + secondNumber;
                        break;
                    case "−":
                        result = firstNumber - secondNumber;
                        break;
                    case "×":
                        result = firstNumber * secondNumber;
                        break;
                    case "÷":
                        if (secondNumber == 0)
                            throw new DivideByZeroException();
                        result = firstNumber / secondNumber;
                        break;
                }

                // Tampilkan ekspresi lengkap sebelum hasil, mis. "10 + 20 ="
                lblExpression.Text = firstNumber + " " + operation + " " + secondNumber + " =";
                txtDisplay.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        // Mereset kalkulator ke kondisi awal
        private void btnClear_Click(object sender, EventArgs e)
        {
            firstNumber = 0;
            secondNumber = 0;
            result = 0;
            operation = "";
            lblExpression.Text = "";
            txtDisplay.Text = "0";
        }

        // Menambahkan titik desimal (mencegah lebih dari satu titik)
        private void btnDecimal_Click(object sender, EventArgs e)
        {
            if (!txtDisplay.Text.Contains("."))
                txtDisplay.Text += ".";
        }
    }
}