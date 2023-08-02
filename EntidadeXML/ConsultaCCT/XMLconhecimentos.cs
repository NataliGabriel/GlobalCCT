using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLB.CCT.EntidadeXML.ConsultaCCT
{
    public class XMLconhecimentos
    {
        public XMLconhecimentos()
        {
            classificacoesMercadoriaManifestoVoo = new XMLclassificacoesMercadoriaManifestoVoo();
            manuseiosEspeciaisManifestoVoo = new XMLmanuseiosEspeciaisManifestoVoo();
            outrasInfosServicoManifestoVoo = new XMLoutrasInfosServicoManifestoVoo();
            solicitacoesServicosEspeciaisManifestoVoo = new XMLsolicitacoesServicosEspeciaisManifestoVoo();
        }
        public XMLclassificacoesMercadoriaManifestoVoo classificacoesMercadoriaManifestoVoo { get; set; }
        public string cnpjResponsavelArquivo { get; set; }
        public string dataEmissao { get; set; }
        public string descricaoMercadoriaManifestoVoo { get; set; }
        public string identificacao { get; set; }
        public string indicadorParcialidade { get; set; }
        public XMLmanuseiosEspeciaisManifestoVoo manuseiosEspeciaisManifestoVoo { get; set; }
        public XMLoutrasInfosServicoManifestoVoo outrasInfosServicoManifestoVoo { get; set; }
        public string pesoBrutoManifestoVoo { get; set; }
        public string quantidadeVolumesManifestoVoo { get; set; }
        public XMLsolicitacoesServicosEspeciaisManifestoVoo solicitacoesServicosEspeciaisManifestoVoo { get; set; }
        public string tipoConhecimento { get; set; }
    }
    public class XMLclassificacoesMercadoriaManifestoVoo
    {
        public string codigo { get; set; }
    }
    public class XMLmanuseiosEspeciaisManifestoVoo
    {
        public string codigo { get; set; }
        public string detalhes { get; set; }
    }
    public class XMLoutrasInfosServicoManifestoVoo
    {
        public string codigo { get; set; }
        public string detalhes { get; set; }
    }
    public class XMLsolicitacoesServicosEspeciaisManifestoVoo
    {
        public string codigo { get; set; }
        public string detalhes { get; set; }
    }
}
