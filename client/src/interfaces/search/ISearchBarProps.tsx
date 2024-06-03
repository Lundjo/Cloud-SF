/**
 * Interface for props of the SearchBar component.
 */
interface ISearchBarProps {
  /**
   * Placeholder text for the search input.
   * @default "Search..."
   */
  placeholder?: string;

  /**
   * Initial value of the search query.
   * @default ""
   */
  initialQuery?: string;

  /**
   * Callback function triggered when the user performs a search.
   * @param query The search query entered by the user.
   */
  onSearch: (query: string) => void;
}

export default ISearchBarProps;
