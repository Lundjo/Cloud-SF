import React, { useState } from "react";
import ISearchBarProps from "../../interfaces/search/ISearchBarProps";

const SearchBar: React.FC<ISearchBarProps> = ({
  placeholder = "",
  initialQuery = "",
  onSearch,
}) => {
  const [query, setQuery] = useState(initialQuery);

  const handleSearchInputChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setQuery(event.target.value);
  };

  const handleSearchSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    onSearch(query);
    setQuery("");
  };

  return (
    <form onSubmit={handleSearchSubmit}>
      <input
        type="text"
        placeholder={placeholder}
        style={{width: '50vw'}}
        className="px-4 py-2 rounded-full border bg-gray-100 border-gray-300 focus:outline-none focus:ring-1 focus:ring-primary-500 h-10"
        value={query}
        onChange={handleSearchInputChange}
      />
    </form>
  );
};

export default SearchBar;
