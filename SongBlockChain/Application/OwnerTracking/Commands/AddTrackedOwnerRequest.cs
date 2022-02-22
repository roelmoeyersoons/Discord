using MediatR;
using Song.Application.Core;
using Song.Model.Data;
using SongBlockChain.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SongBlockChain.Application.OwnerTracking.Commands
{
    public class AddTrackedOwnerRequest : IRequest<Result>
    {
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPubKey { get ; set; }

    }

    public class AddTrackedOwnerRequestHandler : IRequestHandler<AddTrackedOwnerRequest, Result>
    {
        private readonly SongOwnerContext _context;
        private readonly IMediator _mediator;

        public AddTrackedOwnerRequestHandler(SongOwnerContext context, IMediator mediator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Result> Handle(AddTrackedOwnerRequest request, CancellationToken cancellationToken)
        {
            //do validation

            var newUser = new Owner
            {
                OwnerId = request.OwnerId,
                OwnerName = request.OwnerName,
                OwnerKey = request.OwnerPubKey,
            };

            var foundUser = await _context.Users.FindAsync(newUser.OwnerId);

            if (foundUser != null)
                return new Result(false, "The user was already in the database");

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return new Result(true);
        }
    }
}
