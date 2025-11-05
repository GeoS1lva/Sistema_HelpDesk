import { Link, useLocation } from "react-router-dom";
import { Building, Ticket, UserCircle, LogOut } from "lucide-react";
import { useAuth } from "../../contexts/AuthContext";
import logo from "../../assets/logo.png";

const SidebarLink = ({ to, icon: Icon, children, isExpanded }) => {
  const location = useLocation();
  const isActive = location.pathname.startsWith(to);

  return (
    <Link
      to={to}
      className={`
        flex items-center space-x-3 px-3 py-3 rounded-lg transition-all
        ${isExpanded ? "w-full" : "w-full justify-center"} 
        ${
          isActive
            ? "bg-[#381178] text-white "
            : "text-white hover:bg-purple-600 hover:text-white"
        }    
      `}
    >
      <Icon className="w-5 h-5 flex-shrink-0" />
      <span
        className={`font-medium truncante transition-all duration-200 ${
          isExpanded ? "opacity-100 max-w-xs" : "opacity-0 max-w-0"
        }`}
      >
        {children}
      </span>
    </Link>
  );
};

const Sidebar = () => {
  const { user, logout, isSidebarExpanded, expandSidebar, collapseSidebar } =
    useAuth();

  return (
    <div
      onMouseEnter={expandSidebar}
      onMouseLeave={collapseSidebar}
      className={`
        h-screen bg-gradient-to-t from-[#2F0D68] to-[#5D1ACE] text-white flex flex-col fixed 
        border-r-2 border-white transition-all duration-300 ease-in-out
        z-50 ${isSidebarExpanded ? "w-52" : "w-24"} 
      `}
    >
      <div className={`flex items-center h-20 mb-6 relative justify-center`}>
        <img
          src={logo}
          alt="TechDesk Logo"
          className={`w-24 transition-opacity duration-300 ${
            isSidebarExpanded ? "opacity-100" : "opacity-0"
          }`}
        />
      </div>

      <nav className={`flex-1 space-y-2 px-5`}>
        <SidebarLink to="/tickets" icon={Ticket} isExpanded={isSidebarExpanded}>
          Tickets
        </SidebarLink>

        <SidebarLink
          to="/empresas"
          icon={Building}
          isExpanded={isSidebarExpanded}
        >
          Empresas
        </SidebarLink>
      </nav>

      <div className={`mt-auto pt-4 border-t border-white/40 p-4`}>
        <div
          className={`flex items-center ${
            isSidebarExpanded
              ? "space-x-3"
              : "justify-center flex-col space-y-1"
          }`}
        >
          <UserCircle className="w-10 h-10 text-gray-400 flex-shrink-0" />
          <div
            className={`truncate transition-all duration-200 text-center ${
              isSidebarExpanded ? "opacity-100 max-h-40" : "opacity-0 max-h-0"
            }`}
          >
            <p className="font-semibold text-sm truncate" title={user?.name}>
              {user?.name || "Usuário"}
            </p>
            <p className="text-xs text-gray-400 capitalize">
              {user?.role || "Perfil"}
            </p>
          </div>
        </div>
        <button
          onClick={logout}
          className={`
            flex items-center space-x-3 px-4 py-3 rounded-lg transition-colors w-full mt-2
            text-white hover:bg-red-600/50
            ${isSidebarExpanded ? "" : "justify-center"}
          `}
        >
          <LogOut className="w-5 h-5 flex-shrink-0" />
          {isSidebarExpanded && <span className="font-medium">Sair</span>}
        </button>
      </div>
    </div>
  );
};

export default Sidebar;
