using FluentResults;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuFabricaDDD.Application.Common;
using TuFabricaDDD.Contracts.Repositories;
using TuFabricaDDD.Domain.Entities;
using TuFabricaDDD.Domain.Types;
using TuFabricaDDD.Domain.ValueObjects;
using TuFabricaDDD.GrpcContracts;

namespace TuFabricaDDD.Application.Commands
{
    public class CreateUnitCommandHandler : ICommandHandler<CreateUnitCommand, Guid>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUnitCommandHandler(IRepositoryManager repositoryManager, IUnitOfWork unitOfWork)
        {
            _repositoryManager = repositoryManager ?? throw new ArgumentNullException(nameof(repositoryManager));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result<Guid>> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
        {
            // Map command to value objects
            var location = new Location(request.AreaName, request.CurrentLocationX, request.CurrentLocationY);
            var accessPoint = new AccessPoint(
                request.NetworkLocationConnectedAccessPointSsid,
                request.NetworkLocationConnectedAccessPointChannel
            );
            var networkLocation = new NetworkLocation(request.NetworkLocationIpAddress, accessPoint);

            // Instantiate the correct robot type based on category
            Robot? robot = request.Category switch
            {
                RobotCategory.Humanoid => new Humanoid(
                    Guid.NewGuid(),
                    request.SerialNumber,
                    location,
                    networkLocation
                ),
                RobotCategory.FixedRoboticArm => new FixedRoboticArm(
                    Guid.NewGuid(),
                    request.SerialNumber,
                    location,
                    networkLocation,
                    request.ArmLengthInMeters ?? 0,
                    request.PayloadCapacityInKg ?? 0,
                    request.MountingPointId ?? string.Empty
                ),
                RobotCategory.LogisticsMobile => new LogisticsMobile(
                Guid.NewGuid(),
                request.SerialNumber,
                location,
                networkLocation,
                request.MaxPayloadCapacityInKg // Correct argument for 'double'    
                ),
                RobotCategory.CleaningMobile => new CleaningMobile(
                Guid.NewGuid(),
                request.SerialNumber,
                location,
                networkLocation,
                new List<string>() // Provide an empty list or populate with default cleaning tools
                ),
                _ => null
            };

            if (robot == null)
                return Result.Fail<Guid>("Unsupported robot category.");

            // Add to repository (use the correct repository for the type)
            switch (robot)
            {
                case Humanoid h:
                    await _repositoryManager.Humanoids.AddAsync(h);
                    break;
                case FixedRoboticArm arm:
                    await _repositoryManager.FixedRoboticArms.AddAsync(arm);
                    break;
                case LogisticsMobile logistics:
                    await _repositoryManager.LogisticsMobiles.AddAsync(logistics);
                    break;
                case CleaningMobile cleaning:
                    await _repositoryManager.CleaningMobiles.AddAsync(cleaning);
                    break;
                default:
                    await _repositoryManager.Robots.AddAsync(robot);
                    break;
            }

            await _unitOfWork.SaveChangesAsync();

            return Result.Ok(robot.Id);
        }
    }

    public class UpdateUnitCommandHandler : ICommandHandler<UpdateUnitCommand>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUnitCommandHandler(IRepositoryManager repositoryManager, IUnitOfWork unitOfWork)
        {
            _repositoryManager = repositoryManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
        {
            // Example: update logic for Humanoid, extend for other categories as needed
            Robot? robot = await _repositoryManager.Robots.GetByIdAsync(request.Id);
            if (robot == null)
                return Result.Fail("Unit not found.");

            // Update location
            var location = new Location(request.AreaName, request.CurrentLocationX, request.CurrentLocationY);
            var locationResult = robot.UpdateLocation(location);
            if (locationResult.IsFailed)
                return Result.Fail(locationResult.Errors);

            // Update network location
            var accessPoint = new AccessPoint(
                request.NetworkLocationConnectedAccessPointSsid,
                request.NetworkLocationConnectedAccessPointChannel
            );
            var networkLocation = new NetworkLocation(request.NetworkLocationIpAddress, accessPoint);
            var networkResult = robot.UpdateNetworkLocation(networkLocation);
            if (networkResult.IsFailed)
                return Result.Fail(networkResult.Errors);


            await _repositoryManager.Robots.UpdateAsync(robot);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }
    }

    public class DeleteUnitCommandHandler : ICommandHandler<DeleteUnitCommand>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUnitCommandHandler(IRepositoryManager repositoryManager, IUnitOfWork unitOfWork)
        {
            _repositoryManager = repositoryManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
        {
            await _repositoryManager.Robots.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
    }
}