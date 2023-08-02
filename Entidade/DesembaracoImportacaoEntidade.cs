using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.Entidade
{
    public class DesembaracoImportacaoEntidade
    {
        public DesembaracoImportacaoEntidade()
        {
            ListaDeVolumes = new List<VolumesDIEntidade>();
        }
        public string N_REFERENCIA { get; set; }
        public int? CODIGO_LOCAL_EMBARQUE { get; set; }
        public DateTime? DATA_CHEGADA { get; set; }
        public DateTime? DATA_PREV_CHEGADA { get; set; }
        public DateTime? DATA_PREV_EMBARQUE { get; set; }
        public string CODIGO_RECINTO { get; set; }
        public string NR_TERMO { get; set; }
        public double? PESO_BRUTO { get; set; }
        public DateTime? DATA_EMB { get; set; }
        public string NR_VEICULO_TRANSP { get; set; }
        public List<VolumesDIEntidade> ListaDeVolumes { get; set; }


    }
}
