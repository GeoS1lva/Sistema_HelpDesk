import React  from "react";

 
/**
 * @param {{
 *  children: React.ReactNode;
 *  onClick?: () => void;
 *  type?: "button" | "submit" | "reset";
 * }} props
 */

const Button = ({ children, onClick, type = 'button' }) => {
    return (
        <button
            type={type}
            onClick={onClick}
            className="mt-8 w-[268px] h-[40px] bg-gradient-to-r from-[#5D1ACE] to-[#2F0D68] justify-center items-center flex text-white font-bold py-1 px-4 rounded-3xl hover:opacity-90 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-gray-800 focus:ring-purple-500 transition-all duration-300"
        >
            {children}
        </button>
    );gg
};

export default Button;
