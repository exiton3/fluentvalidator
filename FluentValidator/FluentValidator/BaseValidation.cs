using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentValidator.Validators;

namespace FluentValidator
{
    public abstract class  BaseValidator<TEntity> where TEntity:class 
    {
        readonly IList<IValidator> _validators = new List<IValidator>();

        protected INumericValidatorOptions<int> RuleFor(Expression<Func<TEntity, int>> getterExpression)
        {
            var getter = PropertyExpressionHelper.InitializeGetter(getterExpression);
            var propertyName = PropertyExpressionHelper.GetPropertyName(getterExpression);

            var intValidator = new NumericValidator<int>(o=>getter((TEntity)o), propertyName);

            _validators.Add(intValidator);

            return intValidator;
        }

        protected IStringValidatorOptions<TEntity> RuleFor(Expression<Func<TEntity, string>> getterExpression)
        {
            var getter = PropertyExpressionHelper.InitializeGetter(getterExpression);
            var propertyName = PropertyExpressionHelper.GetPropertyName(getterExpression);

            var intValidator = new StringValidator<TEntity>(o => getter((TEntity)o), propertyName);

            _validators.Add(intValidator);

            return intValidator;
        }


        protected IDateTimeValidatorOptions RuleFor(Expression<Func<TEntity, DateTime>> getterExpression)
        {
            var getter = PropertyExpressionHelper.InitializeGetter(getterExpression);
            var propertyName = PropertyExpressionHelper.GetPropertyName(getterExpression);

            var intValidator = new DateTimeValidator(o => getter((TEntity)o), propertyName);

            _validators.Add(intValidator);

            return intValidator;
        }

        public ValidationResult Validate(TEntity entity)
        {
            foreach (var validator in _validators)
            {
                validator.Validate(entity);
            }
            var validationFailures = _validators.Where(x => !x.IsValid)
                .Select(validator => new ValidationFailure(validator.FieldName, validator.ValidationFailures))
                .ToList();

            return new ValidationResult(validationFailures);
        }
    }
}