using System.Collections;
using System.Collections.Generic;

namespace SqlKata.Net6.Execution
{
    public class PaginationIterator<T> : IEnumerable<PaginationResult<T>>
    {
        public PaginationResult<T> FirstPage { get; set; }
        public PaginationResult<T> CurrentPage { get; set; }

        public IEnumerator<PaginationResult<T>> GetEnumerator()
        {
            CurrentPage = FirstPage;

            yield return CurrentPage;

            while (CurrentPage.HasNext)
            {
                CurrentPage = CurrentPage.Next();
                yield return CurrentPage;
            }

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
