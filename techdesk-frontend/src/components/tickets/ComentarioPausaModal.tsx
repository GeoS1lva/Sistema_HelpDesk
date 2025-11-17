import React, { useState } from "react";
import Modal from "../ui/Modal";
import Button from "../ui/Button";

interface ComentarioPausaModalProps {
  onClose: () => void;
  onSubmit: (comentario: string) => void;
  isLoading: boolean;
}

const ComentarioPausaModal: React.FC<ComentarioPausaModalProps> = ({
  onClose,
  onSubmit,
  isLoading,
}) => {
  const [comentario, setComentario] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!comentario) return;
    onSubmit(comentario);
  };

  return (
    <Modal title="Pausar Chamado" onClose={onClose}>
      <form onSubmit={handleSubmit} className="space-y-4">
        <div className="flex-1">
          <label className="block text-sm font-medium text-gray-300 mb-1">
            Motivo da Pausa (Comentário)
          </label>
          <textarea
            value={comentario}
            onChange={(e) => setComentario(e.target.value)}
            placeholder="Descreva por que o chamado está sendo pausado..."
            rows={4}
            className="w-full bg-[#3B3B3B] border border-gray-600 text-white rounded-lg p-2.5 focus:outline-none focus:border-purple-500"
            required
          />
        </div>

        <div className="flex justify-end gap-4 pt-4">
          <Button
            type="button"
            onClick={onClose}
            className="bg-transparent text-red-500 hover:bg-red-500/10 px-4 py-2"
            disabled={isLoading}
          >
            Cancelar
          </Button>
          <Button
            type="submit"
            className="bg-yellow-600 hover:bg-yellow-700 text-white px-6 py-2"
            disabled={isLoading}
          >
            {isLoading ? "Pausando..." : "Confirmar Pausa"}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default ComentarioPausaModal;
