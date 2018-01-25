using Challenge.SDK.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Client
{
    //class Program
    //{
    //    static WriteTextFileLib writeTextFileLib = new WriteTextFileLib("localhost:5500", "api/request");

    //    static void Main(string[] args)
    //    {
    //        // Delay para esperar o servidor de api ser iniciado
    //        Task.Delay(3000);

    //        Console.WriteLine(AskUser());

    //        // Enquanto o usuario quiser enviar mensagens, ficar dentro do loop abaixo
    //        string confirmado;

    //        do
    //        {
    //            Console.WriteLine("Deseja enviar novamente? (s/n): ");
    //            confirmado = Console.ReadLine();
    //        } while (confirmado == "s");

    //        Console.ReadKey();
    //    }

    //    private static async Task<string> CallWebApi(string message)
    //    {
    //        return await writeTextFileLib.WriteFile("Challenge", "send-write-txt", message);
    //    }

    //    private static string AskUser()
    //    {
    //        Console.WriteLine("Digite uma mensagem para ser enviada:");
    //        var mensagem = Console.ReadLine();
    //        var arquivo = CallWebApi(mensagem).Result;
    //        return arquivo;
    //    }
    //}
}
