using HbaseReportService.Hbase.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HbaseReportService.Hbase.Filters
{
    public abstract class FilterBase : Filter
    {

        /**
         * Filters that are purely stateless and do nothing in their reset() methods can inherit this
         * null/empty implementation. {@inheritDoc}
         */
        public void reset()
        {

        }

        public bool filterRowKey(Cell cell)
        {
            return filterAllRemaining();
        }

        /**
         * Filters that never filter all remaining can inherit this implementation that never stops the
         * filter early. {@inheritDoc}
         */
        public bool filterAllRemaining()
        {
            return false;
        }

        /**
         * By default no transformation takes place {@inheritDoc}
         */
        public Cell transformCell(Cell v)
        {
            return v;
        }

        /**
         * Filters that never filter by modifying the returned List of Cells can inherit this
         * implementation that does nothing. {@inheritDoc}
         */
        public void filterRowCells(List<Cell> ignored)
        {
        }

        /**
         * Filters that never filter by modifying the returned List of Cells can inherit this
         * implementation that does nothing. {@inheritDoc}
         */
        public bool hasFilterRow()
        {
            return false;
        }

        /**
         * Filters that never filter by rows based on previously gathered state from
         * {@link #filterCell(Cell)} can inherit this implementation that never filters a row.
         * {@inheritDoc}
         */
        public bool filterRow()
        {
            return false;
        }

        /**
         * Filters that are not sure which key must be next seeked to, can inherit this implementation
         * that, by default, returns a null Cell. {@inheritDoc}
         */
        public Cell getNextCellHint(Cell currentCell)
        {
            return null;
        }

        /**
         * By default, we require all scan's column families to be present. Our subclasses may be more
         * precise. {@inheritDoc}
         */
        public bool isFamilyEssential(byte[] name)
        {
            return true;
        }

        /**
         * Given the filter's arguments it constructs the filter
         * <p>
         * @param filterArguments the filter's arguments
         * @return constructed filter object
         */
        public static Filter createFilterFromArguments(List<byte[]> filterArguments)
        {
            throw new Exception("This method has not been implemented");
        }

        /**
         * Return filter's info for debugging and logging purpose.
         */
        public String toString()
        {
            return this.GetType().Name;
        }

        /**
         * Return length 0 byte array for Filters that don't require special serialization
         */
        public byte[] toByteArray()
        {
            return new byte[0];
        }

        /**
         * Default implementation so that writers of custom filters aren't forced to implement.
         * @return true if and only if the fields of the filter that are serialized are equal to the
         *         corresponding fields in other. Used for testing.
         */
        bool areSerializedFieldsEqual(Filter other)
        {
            return true;
        }
    }
}
