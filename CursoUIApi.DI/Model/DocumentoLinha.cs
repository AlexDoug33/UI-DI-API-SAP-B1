using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursoUIAPI.DI.Model
{
    public class DocumentoLinha
    {
        public int DocEntry { get; set; }
        public int LinhaGrid { get; set; }
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscPrcnt { get; set; }
        public string WhsCode { get; set; }
    }
}
