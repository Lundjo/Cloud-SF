import React from "react";
import IPopUp from "../../interfaces/popup/IPopUp";

const Popup: React.FC<{ popup: IPopUp }> = ({
  popup: {
    title,
    description,
    titleColor,
    buttonConfirmColor,
    buttonConfirmBackground,
    isOpen,
    onClose,
    onConfirm,
  },
}) => {
  return (
    <>
      {isOpen && (
        <div className="fixed top-0 left-0 z-50 w-full h-full flex justify-center items-center">
          <div className="absolute inset-0 bg-gray-900 bg-opacity-70 backdrop-blur-sm"></div>
          <div className="fixed flex justify-center items-center w-full h-full">
            <div className="rounded-xl bg-white p-8 shadow-md">
              <h2 className={`text-xl font-semibold text-center ${titleColor}`}>
                {title}
              </h2>
              <p className={`mt-4 text-md`}>{description}</p>
              <div className="mt-12 flex gap-4 justify-center">
                <button
                  onClick={onConfirm}
                  type="button"
                  className={`rounded-full ${buttonConfirmBackground + "-600"} ${"hover:" + buttonConfirmBackground + "-500"} px-4 py-2 text-sm font-medium ${buttonConfirmColor}`}
                >
                  Yes, I'm sure
                </button>
                <button
                  type="button"
                  className="rounded-full bg-gray-200/70 hover:bg-gray-200/60 hover:text-gray-500 px-4 py-2 text-sm font-medium text-gray-600"
                  onClick={onClose}
                >
                  No, go back
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </>
  );
};

export default Popup;
