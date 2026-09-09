namespace RecortePoligono
{
    public class Vertice
    {
        public double X { get; set; }
        public double Y { get; set; }
        public bool Dentro { get; set; }

        public Vertice(double x, double y, bool dentro)
        {
            X = x;
            Y = y;
            Dentro = dentro;
        }

        public override string ToString()
        {
            string situacao = Dentro ? "DENTRO" : "FORA";
            return $"({X}, {Y}) - {situacao}";
        }
    }

    public class Poligono
    {
        private List<Vertice> vertices;

        public Poligono()
        {
            vertices = new List<Vertice>();
        }

        public void AdicionarVertice(Vertice vertice)
        {
            vertices.Add(vertice);
        }

        public void ExibirVertices()
        {
            if (vertices.Count == 0)
            {
                Console.WriteLine("Nenhum vértice no polígono.");
                return;
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                Console.WriteLine($"V{i + 1}: {vertices[i]}");
            }
        }

        public Poligono ProcessarRecorte()
        {
            Poligono poligonoRecortado = new Poligono();

            if (vertices.Count == 0)
                return poligonoRecortado;

            Console.WriteLine("\n===== PROCESSAMENTO DO RECORTE =====\n");

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertice atual = vertices[i];

                Vertice proximo = vertices[(i + 1) % vertices.Count];

                Console.WriteLine(
                    $"Aresta: V{i + 1} -> V{((i + 1) % vertices.Count) + 1}"
                );

                Console.WriteLine(
                    $"Transição: {(atual.Dentro ? "DENTRO" : "FORA")} -> " +
                    $"{(proximo.Dentro ? "DENTRO" : "FORA")}"
                );

                if (atual.Dentro && proximo.Dentro)
                {
                    Console.WriteLine("Caso: DENTRO -> DENTRO");
                    Console.WriteLine("Ação: adicionar o próximo vértice.");

                    poligonoRecortado.AdicionarVertice(proximo);
                }

                else if (atual.Dentro && !proximo.Dentro)
                {
                    Console.WriteLine("Caso: DENTRO -> FORA");
                    Console.WriteLine("Ação: adicionar ponto de interseção.");

                    Vertice intersecao = CriarIntersecao(atual, proximo);

                    poligonoRecortado.AdicionarVertice(intersecao);
                }

                else if (!atual.Dentro && proximo.Dentro)
                {
                    Console.WriteLine("Caso: FORA -> DENTRO");
                    Console.WriteLine(
                        "Ação: adicionar ponto de interseção e próximo vértice."
                    );

                    Vertice intersecao = CriarIntersecao(atual, proximo);

                    poligonoRecortado.AdicionarVertice(intersecao);
                    poligonoRecortado.AdicionarVertice(proximo);
                }

                else
                {
                    Console.WriteLine("Caso: FORA -> FORA");
                    Console.WriteLine("Ação: nenhum vértice será adicionado.");
                }

                Console.WriteLine();
            }

            return poligonoRecortado;
        }

        private Vertice CriarIntersecao(Vertice a, Vertice b)
        {
            double x = (a.X + b.X) / 2;
            double y = (a.Y + b.Y) / 2;

            return new Vertice(x, y, true);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Poligono poligono = new Poligono();

            Console.WriteLine("==================================");
            Console.WriteLine("   ALGORITMO DE RECORTE");
            Console.WriteLine("      DE POLÍGONOS");
            Console.WriteLine("==================================");

            Console.Write("\nDigite a quantidade de vértices: ");
            int quantidade = int.Parse(Console.ReadLine());

            for (int i = 0; i < quantidade; i++)
            {
                Console.WriteLine($"\n--- Vértice {i + 1} ---");

                Console.Write("Digite X: ");
                double x = double.Parse(Console.ReadLine());

                Console.Write("Digite Y: ");
                double y = double.Parse(Console.ReadLine());

                Console.Write("O vértice está DENTRO ou FORA? ");
                string resposta = Console.ReadLine().Trim().ToUpper();

                bool dentro = resposta == "DENTRO";

                Vertice vertice = new Vertice(x, y, dentro);

                poligono.AdicionarVertice(vertice);
            }

            Console.WriteLine("\n===== POLÍGONO ORIGINAL =====\n");

            poligono.ExibirVertices();

            Poligono recortado = poligono.ProcessarRecorte();

            Console.WriteLine("\n===== LISTA FINAL DE VÉRTICES =====\n");

            recortado.ExibirVertices();

            Console.WriteLine("\nPressione qualquer tecla para finalizar...");
            Console.ReadKey();
        }
    }
}