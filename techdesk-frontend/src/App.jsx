import {
  BrowserRouter,
  Routes,
  Route,
  Navigate,
  Outlet,
} from "react-router-dom";
import { useAuth } from "./contexts/AuthContext"
import { AuthProvider } from "./contexts/AuthContext"
import LoginPage from "./pages/LoginPage"
import RedefinicaoDeSenha from "./pages/RedefinicaoDeSenha"
import ProtectedRoute from "./components/auth/ProtectedRoute"
import Empresas from "./pages/Empresas"
import EditarEmpresa from './pages/EditarEmpresa'
import EditarUsuario from './pages/EditarUsuario'
import AlterarSenha from "./pages/AlterarSenha"
import Sidebar from "./components/layout/Sidebar"
import { Tickets } from "lucide-react"

const MainLayout = () => {
  const { isSidebarExpanded } = useAuth()
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

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/redefinir-senha" element={<RedefinicaoDeSenha />} />

          <Route
            path="/"
            element={
              // <ProtectedRoute>
              <MainLayout />
              // </ProtectedRoute>
            }
          >
            <Route index element={<Navigate to="/empresas" replace />} />

            {/* <Route path="Tickets" element={<Tickets />} /> */}
            <Route path="empresas" element={<Empresas />} />
            <Route path="empresas/editar/:id" element={<EditarEmpresa />} />
            <Route path="usuarios/editar/:userId" element={<EditarUsuario />} />
            
            {/* <Route path="chamados" element={<Chamados />} /> */}
            {/* <Route path="usuarios" element={<Usuarios />} /> */}
            {/* <Route path="tecnicos" element={<Tecnicos />} /> */}
            {/* <Route path="mesas" element={<Mesas />} /> */}
          </Route>

          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
