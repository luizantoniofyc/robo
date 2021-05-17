using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Application.Exceptions
{
    public static class ExceptionMessages
    {
        public const string BN001 = "Robô não encontrado.";
        public const string BN002 = "Robô inválido.";
        public const string BN003 = "Ao menos um dos movimentos deve ser informado.";
        public const string BN004 = "Apenas um movimento é aceito por vez.";
        public const string BN005 = "A rotação só é permitida quando a inclinação da cabeça não está para baixo.";
        public const string BN006 = "A progressão dos estados deve ser feita em ordem crescente ou decrescente.";
        public const string BN007 = "O movimento do punho só é permitido quando o cotovelo está fortemente contraído.";
        public const string BN008 = "Não é possível contiuar porque o robô está corrompido.";
        public const string BN009 = "Você passou dos limites e o seu robô foi corrompido!";
    }
}
