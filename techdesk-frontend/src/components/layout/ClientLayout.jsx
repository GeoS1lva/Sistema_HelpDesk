import { Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import { LogOut } from "lucide-react";
import logo2 from "../../assets/logo2.png";

const ClientLayout = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  return (
    <div className="min-h-screen bg-[#262626] text-white">
      <header className="bg-[#1E1E1E] border-b border-gray-700">
        <div className="container mx-auto px-6 py-4 flex justify-between items-center">
          <img src={logo2} alt="TechDesk Logo" className="w-24" />

          <div className="flex items-center space-x-4">
            <div className="text-right">
              <p className="text-sm font-semibold">
                {user?.userName || "Cliente"}
              </p>
              <p className="text-xs text-gray-400">
                {user?.email || "email@cliente.com"}
              </p>
            </div>
            <button
              onClick={logout}
              title="Sair"
              className="p-2 rounded-full text-gray-400 hover:bg-red-500/20 hover:text-red-400 transition-colors"
            >
              <LogOut className="w-5 h-5" />
            </button>
          </div>
        </div>
      </header>

      <main className="container mx-auto p-6">
        <Outlet />
      </main>
    </div>
  );
};

export default ClientLayout;
