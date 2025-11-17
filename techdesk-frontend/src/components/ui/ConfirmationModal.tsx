import Button from "./Button";

interface ConfirmationModalProps {
  title: string;
  message: string;
  onClose: () => void;
  onConfirm: () => void;
  isLoading?: boolean;
}

const ConfirmationModal: React.FC<ConfirmationModalProps> = ({
  title,
  message,
  onClose,
  onConfirm,
  isLoading = false,
}) => {
  return (
    <div className="fixed inset-0 bg-black/60 z-50 flex items-center justify-center p-4">
      <div className="bg-[#262626] text-white p-6 rounded-lg shadow-lg max-w-sm w-full">
        <h2 className="text-xl font-bold mb-4">{title}</h2>
        <p className="text-gray-300 mb-6">{message}</p>

        <div className="flex justify-end gap-4">
          <Button
            type="button"
            variant="secondary"
            onClick={onClose}
            className="text-gray-400 hover:bg-gray-700/50 px-4 py-2"
            disabled={isLoading}
          >
            Cancelar
          </Button>
          <Button
            type="button"
            onClick={onConfirm}
            className="bg-red-600 hover:bg-red-700 text-white px-6 py-2"
            disabled={isLoading}
          >
            {isLoading ? "Deletando..." : "Confirmar Exclusão"}
          </Button>
        </div>
      </div>
    </div>
  );
};

export default ConfirmationModal;
