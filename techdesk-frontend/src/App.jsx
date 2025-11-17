import {
  BrowserRouter,
  Routes,
  Route,
  Navigate,
  Outlet,
} from "react-router-dom";
import { useAuth } from "./contexts/AuthContext";
import { AuthProvider } from "./contexts/AuthContext";
import LoginPage from "./pages/LoginPage";
import RedefinicaoDeSenha from "./pages/RedefinicaoDeSenha";
import AlterarSenhaPage from "./pages/AlterarSenha"; // 1. IMPORTAR A NOVA PÁGINA
import ProtectedRoute from "./components/auth/ProtectedRoute";
import Sidebar from "./components/layout/Sidebar";

import Empresas from "./pages/Empresas";
import EditarEmpresa from "./pages/EditarEmpresa";
import EditarUsuario from "./pages/EditarUsuario";
import Tickets from "./pages/Tickets";
import Tecnicos from "./pages/Tecnicos";
import Mesas from "./pages/Mesas";
import Categorias from "./pages/Categorias";

import ClientLayout from "./components/layout/ClientLayout";
import ClientDashboard from "./pages/ClientDashboard";
import EditarCategoria from "./pages/EditarCategoria";
import EditarTecnico from "./pages/EditarTecnico";
import DetalhesTicket from "./pages/DetalhesTicket";
import ClientTicketDetalhes from "./pages/ClientTicketDetalhes";

const MainLayout = () => {
  const { isSidebarExpanded } = useAuth();
  return (
    <div className="flex h-screen bg-[#262626]">
      <Sidebar />
      <main
        className={`flex-1 overflow-auto transition-all duration-300 ease-in-out ${
          isSidebarExpanded ? "ml-52" : "ml-24"
        }`}
      >
        <Outlet />
      </main>
    </div>
  );
};

const AppRoutes = () => {
  return (
    <Routes>
      {/* --- ROTAS PÚBLICAS --- */}
      <Route path="/login" element={<LoginPage />} />
      <Route path="/redefinir-senha" element={<RedefinicaoDeSenha />} />
      {/* 2. ADICIONAR A NOVA ROTA AQUI */}
      <Route path="/alterar-senha" element={<AlterarSenhaPage />} />


      {/* --- ROTAS DO ADMIN / TECNICO --- */}
      <Route
        path="/"
        element={
          <ProtectedRoute roles={["Admin", "Tecnico"]}>
            <MainLayout />
          </ProtectedRoute>
        }
      >
        <Route index element={<Navigate to="/tickets" replace />} />
        <Route path="tickets" element={<Tickets />} />
        <Route path="empresas" element={<Empresas />} />
        <Route path="empresas/editar/:id" element={<EditarEmpresa />} />
        <Route path="usuarios/editar/:userName" element={<EditarUsuario />} />
        <Route path="tecnicos" element={<Tecnicos />} />
        <Route path="/tecnicos/editar/:userName" element={<EditarTecnico />} />
        <Route path="mesas" element={<Mesas />} />
        <Route path="categorias" element={<Categorias />} />
        <Route path="categorias/editar/:id" element={<EditarCategoria />} />
        <Route
          path="/chamados/detalhes/:numeroChamado"
          element={<DetalhesTicket />}
        />
      </Route>

      {/* --- ROTAS DO CLIENTE --- */}
      <Route
        path="/portal"
        element={
          <ProtectedRoute roles={["Cliente"]}>
            <ClientLayout />
          </ProtectedRoute>
        }
      >
        <Route index element={<Navigate to="/portal/dashboard" replace />} />
        <Route path="dashboard" element={<ClientDashboard />} />
        <Route path="chamado/:numeroChamado" element={<ClientTicketDetalhes />} />
      </Route>

      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
};

function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <AppRoutes />
      </AuthProvider>
    </BrowserRouter>
  );
}

export default App;