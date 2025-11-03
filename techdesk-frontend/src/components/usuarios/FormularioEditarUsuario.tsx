import React, { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import { ChevronLeft } from "lucide-react"
import Input from "../ui/Input"
import Button from "../ui/Button"


interface UsuarioViewData {
  id: number
  nome: string
  userName: string
  email: string
  status: string
  password: string
}


interface FormularioProps {
  initialData: UsuarioViewData
  onSave: (formData: UsuarioViewData) => Promise<string | void>
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

const FormularioEditarUsuario: React.FC<FormularioProps> = ({
  initialData,
  onSave,
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
      formData.userName !== initialData.userName ||
      formData.email !== initialData.email ||
      formData.status !== initialData.status
    setIsDirty(hasChanged)
  }, [formData, initialData])


  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target
    setFormData((prev) => ({ ...prev, [name]: value }))
  }


  const handleCancel = () => {
    setFormData(initialData)
    setError(null)
  }

  
  const handleSave = async (e: React.FormEvent) => {
    e.preventDefault()
    setIsLoading(true)
    setError(null)

    
    const saveError = await onSave(formData)
    
    
    if (saveError) {
      setError(saveError)
    }
    
    setIsLoading(false)
  }

  return (
    <form onSubmit={handleSave} className="space-y-4 text-white">
      <LabeledInput
        label="Nome Completo"
        name="nome"
        type="text"
        value={formData.nome}
        onChange={handleChange}
        className="h-[42px] bg-[#606060] border border-[#CAC9CF] focus:outline-none focus:border-purple-500 text-sm"
      />

      <div className="flex flex-col md:flex-row gap-4">
        <LabeledInput
          label="Usuário (Login)"
          name="userName"
          type="text"
          value={formData.userName}
          onChange={handleChange}
          className="h-[42px] bg-[#606060] border border-[#CAC9CF] focus:outline-none focus:border-purple-500 text-sm"
        />
        <LabeledInput
          label="Senha"
          name="senha"
          type="password"
          value={formData.password}
          onChange={handleChange}
          className="h-[42px] bg-[#606060] border border-[#CAC9CF] focus:outline-none focus:border-purple-500 text-sm"
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
  );
};

export default FormularioEditarUsuario;
