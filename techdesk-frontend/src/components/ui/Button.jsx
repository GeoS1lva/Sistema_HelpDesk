import React from "react";

/**
 * @param {{
 * children: React.ReactNode;
 * onClick?: () => void;
 * type?: "button" | "submit" | "reset";
 * variant?: "primary" | "secondary";
 * className?: string;
 * disabled?: boolean;
 * }} props
 */
const Button = ({ children, onClick, type = 'button', variant = 'primary', className = '' }) => {
    const baseStyles = "flex justify-center items-center text-white font-bold py-3 px-4 rounded-3xl hover:opacity-90 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-gray-800 transition-all duration-300";

    const variantStyles = {
        
        primary: "bg-gradient-to-r from-[#5D1ACE] to-[#2F0D68] focus:ring-purple-500",
        secondary: "bg-gradient-to-r from-[#383838] to-[#848383]",
    };

    const combinedClasses = `${baseStyles} ${variantStyles[variant]} ${className}`.trim();

    return (
        <button
            type={type}
            onClick={onClick}
            className={combinedClasses}
        >
            {children}
        </button>
    );
};

export default Button;