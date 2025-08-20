using System.Net;
using System.Net.Sockets;
using System.Text;

int port = 35000;

Thread servidorThread = new Thread(() => IniciarServidor(port));
servidorThread.Start();

Thread.Sleep(3000);

// Inicia o cliente
//IniciarCliente("177.195.70.63", port);

Console.WriteLine("Pressione qualquer tecla para sair...");
Console.ReadKey();

static void IniciarServidor(int porta)
{
    TcpListener servidor = null;
    try
    {
        servidor = new TcpListener(IPAddress.Any, porta);
        servidor.Start();
        Console.WriteLine($"[Servidor] Escutando na porta {porta}...");

        while (true)
        {
            TcpClient cliente = servidor.AcceptTcpClient();
            Console.WriteLine("[Servidor] Cliente conectado!");

            NetworkStream stream = cliente.GetStream();
            byte[] buffer = new byte[1024];
            int bytesLidos = stream.Read(buffer, 0, buffer.Length);
            string mensagem = Encoding.UTF8.GetString(buffer, 0, bytesLidos);

            Console.WriteLine($"[Servidor] Recebido: {mensagem}");

            // Responde ao cliente
            string resposta = "Mensagem recebida!";
            byte[] dadosResposta = Encoding.UTF8.GetBytes(resposta);
            stream.Write(dadosResposta, 0, dadosResposta.Length);

            cliente.Close();
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("[Servidor] Erro: " + e.Message);
    }
    finally
    {
        servidor?.Stop();
    }
}

static void IniciarCliente(string host, int porta)
{
    try
    {
        TcpClient cliente = new TcpClient(host, porta);
        Console.WriteLine("[Cliente] Conectado ao servidor!");

        NetworkStream stream = cliente.GetStream();
        string mensagem = "Olá, servidor!";
        byte[] dados = Encoding.UTF8.GetBytes(mensagem);

        // Envia
        stream.Write(dados, 0, dados.Length);
        Console.WriteLine($"[Cliente] Enviado: {mensagem}");

        // Recebe resposta
        byte[] buffer = new byte[1024];
        int bytesLidos = stream.Read(buffer, 0, buffer.Length);
        string resposta = Encoding.UTF8.GetString(buffer, 0, bytesLidos);

        Console.WriteLine($"[Cliente] Resposta: {resposta}");

        cliente.Close();
    }
    catch (Exception e)
    {
        Console.WriteLine("[Cliente] Erro: " + e.Message);
    }
}