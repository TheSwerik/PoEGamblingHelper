SELECT *
FROM "GemData" g
WHERE LOWER("Name") LIKE '%{query.SearchText}%'
  AND ('{query.GemType}' = 'Awakened' AND "Name" LIKE 'Awakened%'
    OR '{query.GemType}' = 'Exceptional' AND
       ("Name" LIKE '%Enhance%' OR "Name" LIKE '%Empower%' OR "Name" LIKE '%Enlighten%')
    OR '{query.GemType}' = 'Skill' AND "Name" NOT LIKE '%Support'
    OR '{query.GemType}' = 'RegularSupport' AND "Name" LIKE '%Support'
    OR '{query.GemType}' = 'All'
    )
  AND (SELECT coalesce(MIN("ChaosValue"), 0)
       FROM "GemTradeData" t
       WHERE t."Name" = g."Name"
         AND NOT "Corrupted"
         AND "GemLevel" = (
           CASE
               WHEN "Name" LIKE 'Awakened%' AND
                    ("Name" LIKE '%Enhance%' OR "Name" LIKE '%Empower%' OR "Name" LIKE '%Enlighten%') THEN 4
               WHEN "Name" LIKE 'Awakened%' THEN 5
               WHEN ("Name" LIKE '%Enhance%' OR "Name" LIKE '%Empower%' OR "Name" LIKE '%Enlighten%') THEN 3
               ELSE 20
               END
           )) BETWEEN
    {query.PricePerTryFrom}
  AND {query.PricePerTryTo}
  AND ({! query.OnlyShowProfitable}
   OR 0 <= ((
    (SELECT coalesce (MIN ("ChaosValue")
    , 0) FROM "GemTradeData" t
    WHERE t."Name" = g."Name"
  AND "Corrupted"
  AND "GemLevel" = (
    CASE
    WHEN "Name" LIKE 'Awakened%'
  AND ("Name" LIKE '%Enhance%'
   OR "Name" LIKE '%Empower%'
   OR "Name" LIKE '%Enlighten%') THEN 3
    WHEN "Name" LIKE 'Awakened%' THEN 4
    WHEN ("Name" LIKE '%Enhance%'
   OR "Name" LIKE '%Empower%'
   OR "Name" LIKE '%Enlighten%') THEN 2
    ELSE 19
    END
    )
    ) + 2 * (SELECT coalesce (MIN ("ChaosValue")
    , 0) FROM "GemTradeData" t
    WHERE t."Name" = g."Name"
  AND "Corrupted"
  AND "GemLevel" = (
    CASE
    WHEN "Name" LIKE 'Awakened%'
  AND ("Name" LIKE '%Enhance%'
   OR "Name" LIKE '%Empower%'
   OR "Name" LIKE '%Enlighten%') THEN 4
    WHEN "Name" LIKE 'Awakened%' THEN 5
    WHEN ("Name" LIKE '%Enhance%'
   OR "Name" LIKE '%Empower%'
   OR "Name" LIKE '%Enlighten%') THEN 3
    ELSE 20
    END
    )
    ) + (SELECT coalesce (MIN ("ChaosValue")
    , 0) FROM "GemTradeData" t
    WHERE t."Name" = g."Name"
  AND "Corrupted"
  AND "GemLevel" = (
    CASE
    WHEN "Name" LIKE 'Awakened%'
  AND ("Name" LIKE '%Enhance%'
   OR "Name" LIKE '%Empower%'
   OR "Name" LIKE '%Enlighten%') THEN 5
    WHEN "Name" LIKE 'Awakened%' THEN 6
    WHEN ("Name" LIKE '%Enhance%'
   OR "Name" LIKE '%Empower%'
   OR "Name" LIKE '%Enlighten%') THEN 4
    ELSE 21
    END
    )
    )
    ) / 4)
    )