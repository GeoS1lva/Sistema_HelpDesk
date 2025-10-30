import React, { useState, useEffect } from "react"

import { useNavigate } from "react-router-dom"
import apiClient from "../../api/apiClient,"

import Input from "../ui/Input"
import Button from "../ui/Button"


const USE_MOCK_DATA_PUT = true


interface FormularioProps {
  initialData: {
    id: number
    nome: string
    cnpj: string
    email: string
    status: string
    dataCadastro: string
  }
}

const LabeledInput: React.FC<{ label: string; [key: string]: any }> = ({
  label,
  ...props
}) => (
  <div className="flex-1">
    <label className="block text-sm font-medium text-gray-300 mb-1">
      {label}
    </label>
    <Input {...props} />
  </div>
);

const FormularioEditarEmpresa: React.FC<FormularioProps> = ({
  initialData,
}) => {
  const [formData, setFormData] = useState(initialData)
  const [isDirty, setIsDirty] = useState(false)
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const navigate = useNavigate()

  
  useEffect(() => {
    setFormData(initialData)
    setIsDirty(false)
  }, [initialData])


  useEffect(() => {
    
    const hasChanged =
      formData.nome !== initialData.nome ||
      formData.email !== initialData.email ||
      formData.status !== initialData.status
    setIsDirty(hasChanged)
  }, [formData, initialData])

  
  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target
    setFormData((prev) => ({ ...prev, [name]: value }));
  }

  
  const handleCancel = () => {
    setFormData(initialData)
    setError(null)
    
  }

  
  const handleSave = async (e: React.FormEvent) => {
    e.preventDefault()
    setIsLoading(true)
    setError(null)

    
    const payload = {
      Nome: formData.nome,
      Email: formData.email,
      Status: formData.status === "Ativo" ? 1 : 0,
    }

    try {
      if (USE_MOCK_DATA_PUT) {
        console.log("Modo Mock: Simulando PUT", payload)
        await new Promise((res) => setTimeout(res, 1000))
      } else {
        await apiClient.put(`/api/empresas/${formData.id}`, payload)
      }

      
      setIsDirty(false)
      setIsLoading(false)
      navigate("/empresas")
    } catch (err) {
      console.error("Erro ao salvar alterações:", err)
      setError("Falha ao salvar. Tente novamente.")
      setIsLoading(false)
    }
  }

  return (
    <div className="bg-[#1E1E1E] p-6 rounded-lg shadow-lg border border-gray-800">
      <h2 className="text-2xl font-bold mb-6">Empresa: {initialData.nome}</h2>

      <form onSubmit={handleSave} className="space-y-4">
        <LabeledInput
          label="Nome Fantasia"
          name="nome"
          type="text"
          value={formData.nome}
          onChange={handleChange}
          className="h-[42px] bg-[#606060] text-sm border border-[#CAC9CF] focus:outline-none focus:border-purple-500"
        />

        <div className="flex flex-col md:flex-row gap-4">
          <LabeledInput
            label="CNPJ"
            name="cnpj"
            type="text"
            value={formData.cnpj}
            disabled
            className="h-[42px] bg-[#3B3B3B] border border-gray-600 cursor-not-allowed text-sm"
          />
          <LabeledInput
            label="Data de Cadastro"
            name="dataCadastro"
            type="date"
            value={formData.dataCadastro.split("T")[0]} 
            disabled
            className="h-[42px] bg-[#3B3B3B] text-sm border border-gray-600 cursor-not-allowed"
          />
        </div>

        <div className="flex flex-col md:flex-row gap-4 ">
          <LabeledInput
            label="E-mail"
            name="email"
            type="email"
            value={formData.email}
            onChange={handleChange}
            className="h-[42px] bg-[#606060] text-sm border border-[#CAC9CF] focus:outline-none focus:border-purple-500"
          />
          <div className="flex-1">
            <label className="block text-sm font-medium text-gray-300 mb-1">
              Status
            </label>
            <select
              name="status"
              value={formData.status}
              onChange={handleChange}
              className="h-[42px] bg-[#606060] border border-[#CAC9CF] text-white rounded-lg p-2 focus:outline-none focus:border-purple-500 w-full"
            >
              <option value="Ativo">Ativo</option>
              <option value="Inativo">Inativo</option>
            </select>
          </div>
        </div>

        {error && <p className="text-red-400 text-sm text-center">{error}</p>}

        {isDirty && (
          <div className="flex justify-end gap-4 pt-4">
            <Button
              type="button"
              variant="secondary"
              onClick={handleCancel} 
              className="bg-transparent text-red-500 hover:bg-red-500/10 px-4 py-2"
              disabled={isLoading} 
            >
              Cancelar
            </Button>
            <Button
              type="submit"
              className="bg-purple-600 hover:bg-purple-700 text-white px-6 py-2"
              disabled={isLoading}
            >
              {isLoading ? "Salvando..." : "Salvar Alterações"}
            </Button>
          </div>
        )}
      </form>
    </div>
  );
};

export default FormularioEditarEmpresa;
