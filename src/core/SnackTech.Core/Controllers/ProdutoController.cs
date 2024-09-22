using SnackTech.Common.Dto;
using SnackTech.Core.Gateways;
using SnackTech.Core.Presenters;
using SnackTech.Core.UseCases;

namespace SnackTech.Core.Controllers
{
    public static class ProdutoController
    {
        //TODO:Passar a referencia de fonte de dados ao Gateway
        public static async Task<string> CadastrarNovoProduto(ProdutoSemIdDto produtoSemIdDto){
            
            var gateway = new ProdutoGateway();
            
            var produtoDto = new ProdutoDto{ 
                Id = Guid.NewGuid(),
                Categoria = produtoSemIdDto.Categoria,
                Descricao = produtoSemIdDto.Descricao,
                Nome = produtoSemIdDto.Nome,
                Valor = produtoSemIdDto.Valor
            };

            var novoProduto = await ProdutoUseCase.CriarNovoProduto(produtoDto,gateway);
            
            if(novoProduto.TeveSucesso()){
                return ProdutoPresenter.ApresentarProdutoComoJson(novoProduto.Dados);
            }

            if(novoProduto.TeveExcecao()){
                return ErroPresenter.ApresentarErroInternoComoJson(novoProduto.Mensagem,novoProduto.Excecao);
            }

            return ErroPresenter.ApresentarErroDeClienteComoJson(novoProduto.Mensagem);
        } 

        public static async Task<string> EditarProduto(ProdutoDto produtoParaAlterar){
            var gateway = new ProdutoGateway();

            var produtoAlterado = await ProdutoUseCase.EditarProduto(produtoParaAlterar,gateway);

            if(produtoAlterado.TeveSucesso()){
                return ProdutoPresenter.ApresentarOperacaoSucessoSemValor($"Alteração de produto {produtoParaAlterar.Id} realizada com sucesso.");
            }

            if(produtoAlterado.TeveExcecao()){
                return ErroPresenter.ApresentarErroInternoComoJson(produtoAlterado.Mensagem,produtoAlterado.Excecao);
            }

            return ErroPresenter.ApresentarErroDeClienteComoJson(produtoAlterado.Mensagem);
        }    

        public static async Task<string> RemoverProduto(Guid id){
            var gateway = new ProdutoGateway();

            var remocaoProduto = await ProdutoUseCase.RemoverProduto(id,gateway);

             if(remocaoProduto.TeveSucesso()){
                return ProdutoPresenter.ApresentarOperacaoSucessoSemValor($"Alteração de produto {id} realizada com sucesso.");
            }

            if(remocaoProduto.TeveExcecao()){
                return ErroPresenter.ApresentarErroInternoComoJson(remocaoProduto.Mensagem,remocaoProduto.Excecao);
            }

            return ErroPresenter.ApresentarErroDeClienteComoJson(remocaoProduto.Mensagem);
        }  

        public static async Task<string> BuscarProdutosPorCategoria(int categoriaId){
            var gateway = new ProdutoGateway();

            var produtos = await ProdutoUseCase.BuscarProdutoPorCategoria(categoriaId,gateway);

            if(produtos.TeveSucesso()){
                return ProdutoPresenter.ApresentarListaProdutosComoJson(produtos.Dados);
            }

            if(produtos.TeveExcecao()){
                return ErroPresenter.ApresentarErroInternoComoJson(produtos.Mensagem,produtos.Excecao);
            }

            return ErroPresenter.ApresentarErroDeClienteComoJson(produtos.Mensagem);
        } 
    }
}