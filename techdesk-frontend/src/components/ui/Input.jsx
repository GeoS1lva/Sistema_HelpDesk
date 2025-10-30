import React from "react";

/**
 * @param {React.InputHTMLAttributes<HTMLInputElement>} props
 */

const Input = ({ className, ...props }) => {
  const baseStyles =
    "w-full h-9 bg-[#383838] text-[12px] text-[#C5C4CC] placeholder-[#C5C4CC] placeholder:opacity-40 rounded-md px-4 py-2 focus:outline-none focus:ring-2 focus:ring-[#7657AB] transition-shadow";

  const combinedClasses = `${baseStyles} ${className || ""}`.trim();

  return <input {...props} className={combinedClasses} />;
};

export default Input;
