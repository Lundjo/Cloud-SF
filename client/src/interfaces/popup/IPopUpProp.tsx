/**
 * Represents the properties for setting up a popup component.
 */
interface IPopUpProp {
  /**
   * Function to set up the popup with specified parameters.
   * @param title The title of the popup.
   * @param description The description or content of the popup.
   * @param titleColor The color of the title text.
   * @param buttonConfirmColor The color of the confirm button text.
   * @param buttonConfirmBackground The background color of the confirm button.
   * @param onConfirm Callback function to be executed when the confirm button is clicked.
   */
  SetUpPopup: (
    title: string,
    description: string,
    titleColor: string,
    buttonConfirmColor: string,
    buttonConfirmBackground: string,
    onConfirm: () => void
  ) => void;
}

export default IPopUpProp;