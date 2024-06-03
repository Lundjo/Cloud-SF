import IPopUp from "../../interfaces/popup/IPopUp";

const DefaultPopUp: IPopUp = {
  title: "",
  description: "",
  titleColor: "",
  buttonConfirmColor: "",
  buttonConfirmBackground: "",
  isOpen: false,
  onClose: () => {},
  onConfirm: () => {},
};

export default DefaultPopUp;
