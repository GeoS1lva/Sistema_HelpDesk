using Moq;
using Sistema_HelpDesk;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;


namespace Sistema_HelpDesk.Tests
{
    public class RetornarInformacoesUsuarioEmpresaUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserLoginRepository> _loginRepoMock;
        private readonly Mock<IUsuarioEmpresaRepository> _usuarioRepoMock;
        private readonly Mock<IEmpresaRepository> _empresaRepoMock;
        private readonly RetornarInformacoesUsuarioEmpresaUseCase _useCase;

        public RetornarInformacoesUsuarioEmpresaUseCaseTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loginRepoMock = new Mock<IUserLoginRepository>();
            _usuarioRepoMock = new Mock<IUsuarioEmpresaRepository>();
            _empresaRepoMock = new Mock<IEmpresaRepository>();

            _unitOfWorkMock.SetupGet(x => x.UserLoginRepository).Returns(_loginRepoMock.Object);
            _unitOfWorkMock.SetupGet(x => x.UsuarioEmpresaRepository).Returns(_usuarioRepoMock.Object);
            _unitOfWorkMock.SetupGet(x => x.EmpresaRepository).Returns(_empresaRepoMock.Object);

            _useCase = new RetornarInformacoesUsuarioEmpresaUseCase(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Erro_Se_Usuario_Nao_Encontrado()
        {
            // Arrange
            _loginRepoMock.Setup(r => r.RetornarLogin("joao")).ReturnsAsync((UserLogin)null);

            // Act
            var result = await _useCase.RetornarInformacoesUsuarios("joao");

            // Assert
            Assert.False(result.Sucesso);
            Assert.Equal("Usuário não encontrado!", result.ErrorMessage);
        }

        [Fact]
        public async Task Deve_Retornar_Sucesso_Quando_Usuario_Empresa_Encontrados()
        {
            // Arrange
            var login = new UsuarioLogin { Id = 1, UserName = "joao", Email = "joao@teste.com", TipoPerfil = TipoPerfil };
            var user = new UsuarioEmpresa { Id = 1, EmpresaId = 99, Nome = "João da Silva", Status = true };
            var empresa = new Empresa { Id = 99, Nome = "TechCorp" };

            _loginRepoMock.Setup(r => r.RetornarLogin("joao")).ReturnsAsync(login);
            _usuarioRepoMock.Setup(r => r.RetornarUsuario(1)).ReturnsAsync(user);
            _empresaRepoMock.Setup(r => r.RetornarEmpresa(99)).ReturnsAsync(empresa);

            // Act
            var result = await _useCase.RetornarInformacoesUsuarios("joao");

            // Assert
            Assert.True(result.Sucesso);
            Assert.Equal("joao", result.Dados.UserName);
            Assert.Equal("TechCorp", result.Dados.NomeEmpresa);
            Assert.True(result.Dados.Status);
        }
    }
}