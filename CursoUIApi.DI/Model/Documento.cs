using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursoUIAPI.DI.Model
{
    public class Documento
    {
        public Documento()
        {
            Linhas = new List<DocumentoLinha>();
        }
        
        public int DocEntryNF { get; set; }
        public int DocEntryPV { get; set; }
        public string CardCode { get; set; }
        public DateTime DocDate { get; set; }
        public int BPLId { get; set; }
        public int GroupNum { get; set; }

        public string Erro { get; set; }

        public List<DocumentoLinha> Linhas { get; set; }
    }
}
