import React, { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import FormularioEditarEmpresa from '../components/empresas/FormularioEditarEmpresa'
import ListaUsuariosEmpresa from '../components/usuarios/ListaUsuariosEmpresa'
import apiClient from '../api/apiClient,'

interface EmpresaData {
  id: number
  nome: string
  cnpj: string
  dataCadastro: string
  email: string
  status: string
}

interface UsuarioData {
  id: number
  nome: string
  usuario: string
  email: string
  dataCadastro: string
  status: string
}

const mockEmpresasData: EmpresaData = {
  id: 1,
  nome: 'Empresa A',
  cnpj: '00.000.000/0001-00',
  dataCadastro: '2024-05-10',
  email: 'contato@empresaa.com',
  status: 'Ativo',
}

const mockUsuariosData: UsuarioData[] = [
  { id: 1, nome: 'Alan M.', usuario: 'alan.m', email: 'alan@empresa.com', dataCadastro: '10/05/2024', status: 'Ativo' },
  { id: 2, nome: 'Beatriz S.', usuario: 'beatriz.s', email: 'bia@empresa.com', dataCadastro: '11/05/2024', status: 'Inativo' },
];


const EditarEmpresa: React.FC = () => {
  const { id } = useParams<{ id: string }>()
  const empresaId = parseInt(id || "0")
  const [empresa, setEmpresa] = useState<EmpresaData>(mockEmpresasData)
  const [usuarios, setUsuarios] = useState<UsuarioData[]>(mockUsuariosData)
  const [loading, setLoading] = useState(true)

  
  useEffect(() => {
    const fetchDados = async () => {
      setLoading(true);
      try {
        const [empresaRes, usuariosRes] = await Promise.all([
          apiClient.get(`/api/empresas/${id}`),
          apiClient.get(`/api/usuarios?empresaId=${id}`)
        ]);
        setEmpresa(empresaRes.data);
        setUsuarios(usuariosRes.data);
      } catch (error) {
        console.error("Erro ao buscar dados:", error);
      }
      setLoading(false);
    };
    fetchDados();
    setLoading(false) 
  }, [id])

  const handleUsuarioCadastrado = (novoUsuario: UsuarioData) => {
    setUsuarios((usuariosAtuais) => [novoUsuario, ...usuariosAtuais])
  }

  if (loading) {
    return <div className="p-10 text-white">Carregando...</div>
  }

  return (
    <div className="p-10 text-white space-y-10">
      <FormularioEditarEmpresa initialData={empresa} />

      <ListaUsuariosEmpresa 
      usuarios={usuarios} empresaId={empresaId} onUsuarioCadastrado={handleUsuarioCadastrado} />
    </div>
  );
};

export default EditarEmpresa