using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TitaniumColector.Classes.Model
{
    class ProdutoAlocacao :Produto 
    {
        public int CodigoItem { get; private set; }
        public string LocaisLoteProduto { get; set; }
        public string VolumeItem { get; set; }
        public int CodigoLocalAlocacao { get; set; }
        public string NomeLocalAlocacao { get; set; }
        public bool Alocado { get; private set; }

        public ProdutoAlocacao( int codigoItem, string locaisLoteProduto, string volumeItem, int codigoProduto, string partnumber, string descricao, string nomeLocalLote, int codLoteProd)
            : base(codigoProduto,  partnumber,  descricao,  nomeLocalLote,  codLoteProd)
        {
            this.CodigoItem = codigoItem;
            this.LocaisLoteProduto = locaisLoteProduto;
            this.VolumeItem = volumeItem;
            this.Alocado = false;
        }

        public ProdutoAlocacao() : base()
        {
        }
    }
}
