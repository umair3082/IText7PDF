using FluentValidation;
using IText7PdfPOC;

namespace QRCBarcodControlNetPOC
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            // Rules For Parent Properties
            RuleFor(x => x.customerId).NotNull();

            // Custom Rule for Customer Id: Suppose you enter 0
            RuleFor(x => x.customerId)
                .Custom((value, context) =>
                {
                    if (value == 0)
                    {
                        context.AddFailure("Customer Id must be greater than 0");
                    }
                });

            RuleFor(x => x.customerName).NotEmpty().WithMessage("Customer Name is required");
            RuleFor(x => x.customerName).MaximumLength(255).WithMessage("Customer Name should be a maximum of 255 characters");
            RuleFor(x => x.customerEmail).NotEmpty().WithMessage("Customer Email is required");
            RuleFor(x => x.customerEmail).EmailAddress().WithMessage("Invalid Email Address");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
            RuleFor(x => x.Address).MaximumLength(255).WithMessage("Address should be a maximum of 255 characters");

            // Age should be between 18 and 60
            RuleFor(x => x.Age).InclusiveBetween(18, 60).WithMessage("Age should be between 18 and 60");

            // Enforce at least one item in the cart
            RuleFor(x => x.Items).Must(x => x.Count() > 0).WithMessage("Please enter at least one item.");

            // Rules for Child Properties
            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(x => x.ItemId).NotNull();
                items.RuleFor(x => x.ItemName).NotEmpty().NotNull().WithMessage("Item Name is required");
                items.RuleFor(x => x.Quantity).GreaterThanOrEqualTo(1).WithMessage("Quantity is required and must be greater than 0");
                items.RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price is required & must be greater than 0");
                items.RuleFor(x => x.Version).NotEmpty().NotNull()
                    .Matches(@"^[0-9]{1,2}\.[0-9]{1,2}\.[0-9]{1,2}$").WithMessage("Template Version must be in the format x.x.x");
                items.RuleFor(x => x.Color).NotEmpty()
                    .Matches(@"^#(?:[0-9a-fA-F]{3}){1,2}$")
                    .WithMessage("Color must be in valid hexadecimal format (e.g., #fff or #ffffff).");

                // Custom Rule for Color: Black (#000000 or #000) is not allowed
                items.RuleFor(x => x.Color)
                    .Custom((value, context) =>
                    {
                        if (value.ToLower() == "#000000" || value.ToLower() == "#000")
                        {
                            context.AddFailure($"{value} is not allowed");
                        }
                    });

                items.RuleFor(x => x.ImageURL).NotEmpty().NotNull()
                    .Matches(@"\b(?:png|jpg|jpeg)\b")
                    .WithMessage("Image must be in png, jpg, or jpeg format and a valid URL.");
            });
        }
    }
}
