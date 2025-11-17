import React, { useState, useEffect } from "react";
import { TecnicoViewData } from "../../pages/EditarTecnico"; 
import Button from "../ui/Button";
import Input from "../ui/Input";


export interface FormularioPayload {
  nome: string;
  novoUserName: string;
  email: string;
  password?: string;
}

interface FormularioProps {
  initialData: TecnicoViewData;
  onSave: (formData: FormularioPayload) => Promise<string | void>;
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

const FormularioEditarTecnico: React.FC<FormularioProps> = ({
  initialData,
  onSave,
}) => {
  const [formData, setFormData] = useState({
    nome: initialData.nome,
    novoUserName: initialData.userName,
    email: initialData.email,
    password: "",
  });

  const [isDirty, setIsDirty] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setFormData({
      nome: initialData.nome,
      novoUserName: initialData.userName,
      email: initialData.email,
      password: "",
    });
    setIsDirty(false);
  }, [initialData]);

  useEffect(() => {
    const hasChanged =
      formData.nome !== initialData.nome ||
      formData.novoUserName !== initialData.userName ||
      formData.email !== initialData.email ||
      formData.password !== "";
    setIsDirty(hasChanged);
  }, [formData, initialData]);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleCancel = () => {
    setFormData({
      nome: initialData.nome,
      novoUserName: initialData.userName,
      email: initialData.email,
      password: "",
    });
    setError(null);
  };

  
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault(); 
    setIsLoading(true);
    setError(null);

    try {
      const saveError = await onSave(formData); 
      
      if (saveError) {
        setError(saveError);
      }
    } catch (err) {
      console.error("Erro no 'handleSubmit' do formulário:", err);
      setError("Ocorreu um erro inesperado ao salvar.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    
    <form onSubmit={handleSubmit} className="space-y-4 text-white">
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
          name="novoUserName"
          type="text"
          value={formData.novoUserName}
          onChange={handleChange}
          className="h-[42px] bg-[#606060] border border-[#CAC9CF] focus:outline-none focus:border-purple-500 text-sm"
        />
        <LabeledInput
          label="E-mail"
          name="email"
          type="email"
          value={formData.email}
          onChange={handleChange}
          className="h-[42px] bg-[#606060] border border-[#CAC9CF] focus:outline-none focus:border-purple-500 text-sm"
        />
      </div>

      <div className="flex flex-col md:flex-row gap-4">
        <LabeledInput
          label="Nova Senha (Deixe em branco para não alterar)"
          name="password"
          type="password"
          value={formData.password}
          onChange={handleChange}
          className="h-[42px] bg-[#606060] border border-[#CAC9CF] focus:outline-none focus:border-purple-500 text-sm"
        />

        <div className="flex-1">
          <label className="block text-sm font-medium text-gray-300 mb-1">
            Perfil (Não editável)
          </label>
          <Input
            type="text"
            value={initialData.tipoPerfil}
            disabled
            className="h-[42px] bg-[#3B3B3B] border border-gray-600 cursor-not-allowed text-sm capitalize" label={""}          />
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

export default FormularioEditarTecnico;