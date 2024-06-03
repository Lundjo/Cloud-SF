/**
 * Represents the properties of a popup component.
 */
interface IPopUp {
  /**
   * The title of the popup.
   */
  title: string;

  /**
   * The description or content of the popup.
   */
  description: string;

  /**
   * The color of the title text.
   */
  titleColor: string;

  /**
   * The color of the confirm button text.
   */
  buttonConfirmColor: string;

  /**
   * The background color of the confirm button.
   */
  buttonConfirmBackground: string;

  /**
   * Indicates whether the popup is open or closed.
   */
  isOpen: boolean;

  /**
   * Callback function to be executed when the popup is closed.
   */
  onClose: () => void;

  /**
   * Callback function to be executed when the confirm button is clicked.
   */
  onConfirm: () => void;
}

export default IPopUp;
