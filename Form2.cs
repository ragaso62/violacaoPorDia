using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Text.Json;

namespace ViolacoesPorDia
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.Load += Form2_Load;
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            using var client = new HttpClient();
            var json = await client.GetStringAsync("http://localhost:8080/api/violacoes/por-dia");
            var dados = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(json);

            double[] xs = new double[dados.Count];
            double[] ys = new double[dados.Count];
            string[] labels = new string[dados.Count];

            for (int i = 0; i < dados.Count; i++)
            {
                labels[i] = dados[i]["dia"].GetString();
                ys[i] = dados[i]["total"].GetInt32();
                xs[i] = i;
                int indiceInstalacao = Array.IndexOf(labels, "2026-09-06");

                if (indiceInstalacao >= 0)
                {
                    var linha = formsPlot1.Plot.Add.VerticalLine(indiceInstalacao);
                    linha.LineWidth = 1;
                    linha.LinePattern = ScottPlot.LinePattern.Dashed;
                    linha.Color = ScottPlot.Colors.Gray;
                }

                int indicereforco = Array.IndexOf(labels, "2026-09-07");

                if (indicereforco >= 0)
                {
                    var linha = formsPlot1.Plot.Add.VerticalLine(indicereforco);
                    linha.LineWidth = 1;
                    linha.LinePattern = ScottPlot.LinePattern.Dashed;
                    linha.Color = ScottPlot.Colors.Gray;
                }
            }

            formsPlot1.Plot.Add.Scatter(xs, ys);
            formsPlot1.Plot.Axes.Bottom.SetTicks(xs, labels);
            formsPlot1.Plot.Axes.Bottom.TickLabelStyle.Rotation = -45;
            formsPlot1.Plot.Title("Violações por dia");
            formsPlot1.Plot.Axes.AutoScale();
            formsPlot1.Refresh();
            formsPlot1.Plot.Add.Annotation("Anticheat instalado em 06/09 \n\nAnticheat reforçado com kicki automatico em 07/09", ScottPlot.Alignment.UpperRight);
        }
    }
}