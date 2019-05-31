///create_explosion(x, y)
var xx = argument0;
var yy = argument1;

repeat (10) 
{
    instance_create(xx - 16 + random(32), yy - 16 + random(32), o_explosionpiece)
}

part_particles_create(o_particles.system, xx, yy, o_particles.explosion_center_part, 2);
