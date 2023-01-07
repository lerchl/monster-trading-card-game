using MonsterTradingCardGame.Data;

namespace MonsterTradingCardGame.Logic {

    public abstract class Logic<T, E> where T : Repository<E> where E : Entity {

        protected T Repository { get; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public Logic(T repository) {
            Repository = repository;
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////
    
        public E Save(E entity) {
            return Repository.Save(entity);
        }

        public E FindById(Guid id) {
            return Repository.FindById(id);
        }

        public List<E> FindAll() {
            return Repository.FindAll();
        }

        public void Delete(Guid id) {
            Repository.Delete(id);
        }
    }
}
